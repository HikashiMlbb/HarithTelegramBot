using Main.Core.Domain.ValueObjects;
using Telegram.Bot.Types;

namespace Main.Core.Application.Services.Interfaces;

public interface IRewardService
{
    public Task<bool> RewardAsync(Account account, Message message);
}