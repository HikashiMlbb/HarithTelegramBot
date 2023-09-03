using TelegramBot.Domain.Entities;

namespace TelegramBot.Domain.Interfaces;

public interface IBotMembersRepository
{
    // 1. Add new member to database
    /// <summary>
    ///     Adds the member to the database. If user already exists in the database, causes MemberAlreadyExistsException
    /// </summary>
    /// <param name="member">Instance of BotMember type which needs to be added into the database</param>
    /// <returns>Returns BotMember which has been added to the collection.</returns>
    public Task<BotMember> AddAsync(BotMember member);
    
    /// <summary>
    ///     Accepts the user's telegram id and his chat id. It returns the BotMember instance or null if not found
    /// </summary>
    /// <param name="telegramId">An user's telegram id</param>
    /// <param name="chatId">A chat id in which it should find the user</param>
    /// <returns>Returns BotMember which found by it's telegram and chat ids</returns>
    public Task<BotMember?> FindUserByTelegramAndChatIdAsync(long telegramId, long chatId);

    /// <summary>
    ///     Get a list of BotMember instances which has specified chat id. Can be empty.
    /// </summary>
    /// <param name="id">A chat id in which it should find the users</param>
    /// <returns>Returns a list of BotMembers which found by it's chat id</returns>
    public Task<IEnumerable<BotMember>> FindUsersByChatIdAsync(long id);
    
    /// <summary>
    ///     Get a list of BotMember instances which observe given condition
    /// </summary>
    /// <param name="predicate">Condition which should be used in finding users</param>
    /// <returns>Returns a list of BotMembers which found by given condition</returns>
    public Task<IEnumerable<BotMember>> FilterBy(Predicate<BotMember> predicate);
    
    /// <summary>
    ///     Rewards the user by experience. Can return MemberNotFoundException if there's no member in database
    /// </summary>
    /// <param name="member">Instance of BotMember who should be rewarded by experience</param>
    /// <param name="reward">Number which shows how many experience member should be given</param>
    /// <returns>Rewarded BotMember or MemberNotFoundException if there's no member in database</returns>
    public Task<BotMember> RewardMember(BotMember member, float reward);
}