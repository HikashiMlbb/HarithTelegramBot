using TelegramBot.Domain.Entities;
using TelegramBot.Domain.Interfaces;

namespace TelegramBot.Persistence.Repositories;

public class BotMembersRepository : IBotMembersRepository
{
    
    public Task<BotMember> AddAsync(BotMember member)
    {
        throw new NotImplementedException();
    }

    public Task<BotMember?> FindUserByTelegramAndChatIdAsync(long telegramId, long chatId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BotMember>> FindUsersByChatIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BotMember>> FilterBy(Predicate<BotMember> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<BotMember> RewardMember(BotMember member, float reward)
    {
        throw new NotImplementedException();
    }
}