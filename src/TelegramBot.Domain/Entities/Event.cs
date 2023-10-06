using TelegramBot.Domain.Primitives;

namespace TelegramBot.Domain.Entities;

public sealed class Event : Entity
{
    public long ChatId { get; set; }
    public string Name { get; set; }
    public float Multiplier { get; set; }
    
    public Event(string name, long chatId, float multiplier)
    {
        Name = name;
        ChatId = chatId;
        Multiplier = multiplier;
    }
}