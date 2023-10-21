using TelegramBot.Partitions.Shared.Primitives;

namespace Main.Core.Domain.Entities;

public sealed class Event : Entity
{
    public Event(string name, long chatId, float multiplier)
    {
        Name = name;
        ChatId = chatId;
        Multiplier = multiplier;
    }

    public long ChatId { get; set; }
    public string Name { get; set; }
    public float Multiplier { get; set; }
}