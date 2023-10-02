using Telegram.Bot.Types;

namespace Basic.Application.Data.Interfaces;

public interface IRewardingBuilder
{
    public IRewardingBuilder TryReward(Message message);
    public IRewardingBuilder LevelUp();
    public IRewardingBuilder UpdateLastRewardDate();
    public bool Build(out int currentLevel);
}