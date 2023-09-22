using Telegram.Bot.Types;
using TelegramBot.Domain.ValueObjects;

namespace TelegramBot.Application.Data.Interfaces;

public interface IRewardService
{
    public Task<bool> RewardAsync(Account account, Message message);
}