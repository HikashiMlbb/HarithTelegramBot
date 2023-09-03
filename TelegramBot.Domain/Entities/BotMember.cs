using TelegramBot.Domain.Primitives;

namespace TelegramBot.Domain.Entities;

public sealed class BotMember : Entity
{
    public string FirstName { get; set; }
    public long TelegramId { get; set; }
    public long ChatId { get; set; }
    public int Level { get; set; }
    public float Experience { get; set; }
    
    public BotMember(Guid id, string firstName, long telegramId, long chatId) : base(id)
    {
        FirstName = firstName;
        TelegramId = telegramId;
        ChatId = chatId;
        Level = 0;
        Experience = 0f;
    }
}