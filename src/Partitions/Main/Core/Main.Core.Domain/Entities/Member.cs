using Main.Core.Domain.ValueObjects;
using TelegramBot.Partitions.Shared.Primitives;

namespace Main.Core.Domain.Entities;

public sealed class Member : Entity
{
    public Member(string firstName, Account account)
    {
        FirstName = firstName;
        Account = account;
        Level = 0;
        Experience = 0f;
    }
    
    public string FirstName { get; set; }
    public Account Account { get; set; }
    public int Level { get; set; }
    public float Experience { get; set; }
    public float ExperienceToReward { get; set; }
    public DateTime LastMessageAt { get; set; }
    public DateTime LastRewardAt { get; set; }
}