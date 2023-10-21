using Main.Core.Domain.Entities;
using Telegram.Bot.Types;

namespace Main.Core.Application.Services.Interfaces;

public interface IRewardingBuilder
{
    public IRewardingBuilder TryReward(Message message);
    public IRewardingBuilder TryReward(Message message, IEnumerable<Event> events);
    public IRewardingBuilder LevelUp();
    public IRewardingBuilder UpdateLastRewardDate();
    public bool Build();
}