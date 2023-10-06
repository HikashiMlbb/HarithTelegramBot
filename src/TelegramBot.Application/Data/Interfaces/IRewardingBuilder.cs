using Telegram.Bot.Types;

namespace TelegramBot.Application.Data.Interfaces;

public interface IRewardingBuilder
{
    public IRewardingBuilder TryReward(Message message);
    public IRewardingBuilder LevelUp();
    public IRewardingBuilder UpdateLastRewardDate();
    public bool Build();
}