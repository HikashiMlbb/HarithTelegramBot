using Telegram.Bot.Types;
using Basic.Domain.ValueObjects;

namespace Basic.Application.Data.Interfaces;

public interface IRewardService
{
    public Task<bool> RewardAsync(Account account, Message message);
}