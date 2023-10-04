namespace TelegramBot.Domain.Exceptions.Events;

[Serializable]
public class EventNotFoundException : Exception
{
    public EventNotFoundException(string name, long chatId) : base($"Event \"{name}\" in chat \"{chatId}\" doesn't exist")
    {
        
    }
}