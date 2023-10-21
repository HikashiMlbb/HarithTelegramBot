using Main.Core.Domain.Entities;
using Main.Core.Domain.ValueObjects;

namespace Main.Core.Domain.Repositories;

public interface IMembersRepository
{
    // 1. Add new member to database
    /// <summary>
    ///     Adds the member to the database. If user already exists in the database, causes MemberAlreadyExistsException
    /// </summary>
    /// <param name="member">Instance of BotMember type which needs to be added into the database</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Returns BotMember which has been added to the collection.</returns>
    public Task<Member> AddAsync(Member member, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Accepts the user's telegram id and his chat id. It returns the BotMember instance or null if not found
    /// </summary>
    /// <param name="account"></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Returns BotMember which found by it's telegram and chat ids</returns>
    public Task<Member?> FindUserByAccountAsync(Account account, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get a list of BotMember instances which has specified chat id. Can be empty.
    /// </summary>
    /// <param name="id">A chat id in which it should find the users</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Returns a list of BotMembers which found by it's chat id</returns>
    public Task<IEnumerable<Member>> FindUsersByChatIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get a list of BotMember instances which observe given condition
    /// </summary>
    /// <param name="predicate">Condition which should be used in finding users</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Returns a list of BotMembers which found by given condition</returns>
    public Task<IEnumerable<Member>> FilterByAsync(Predicate<Member> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Rewards the user by experience. Can return MemberNotFoundException if there's no member in database
    /// </summary>
    /// <param name="member">Instance of BotMember who should be rewarded by experience</param>
    /// <param name="reward">Number which shows how many experience member should be given</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Rewarded BotMember or MemberNotFoundException if there's no member in database</returns>
    public Task<Member> RewardMember(Member member, float reward, CancellationToken cancellationToken = default);
}