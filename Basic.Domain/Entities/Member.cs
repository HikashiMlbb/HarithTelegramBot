using Basic.Domain.ValueObjects;

namespace Basic.Domain.Entities;

public sealed class Member
{
    public Member(string firstName, Account account)
    {
        FirstName = firstName;
        Account = account;
        Level = 0;
        Experience = 0f;
    }

    public int Id { get; set; }
    public string FirstName { get; set; }
    public Account Account { get; set; }
    public int Level { get; set; }
    public float Experience { get; set; }
    public float ExperienceToReward { get; set; }
    public DateTime LastMessageAt { get; set; }
    public DateTime LastRewardAt { get; set; }
}