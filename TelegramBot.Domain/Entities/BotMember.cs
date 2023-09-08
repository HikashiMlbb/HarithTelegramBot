using TelegramBot.Domain.Primitives;
using TelegramBot.Domain.ValueObjects;

namespace TelegramBot.Domain.Entities;

public sealed class BotMember : Entity
{
    public string FirstName { get; set; }
    public Account Account { get; set; }
    public int Level { get; set; }
    public float Experience { get; set; }

    public BotMember() : base(Guid.NewGuid())
    {
        FirstName = String.Empty;
        Account = new Account();
        Level = 0;
        Experience = 0;
    }
    
    public BotMember(string firstName, Account account) : base(Guid.NewGuid())
    {
        FirstName = firstName;
        Account = account;
        Level = 0;
        Experience = 0f;
    }
}