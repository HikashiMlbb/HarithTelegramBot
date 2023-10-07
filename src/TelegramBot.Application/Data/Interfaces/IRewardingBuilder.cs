using Telegram.Bot.Types;
using TelegramBot.Domain.Entities;

namespace TelegramBot.Application.Data.Interfaces;

public interface IRewardingBuilder
{
    public IRewardingBuilder TryReward(Message message);
    public IRewardingBuilder TryReward(Message message, IEnumerable<Event> events);
    public IRewardingBuilder LevelUp();
    public IRewardingBuilder UpdateLastRewardDate();
    public bool Build();
}