namespace Main.Core.Domain.Exceptions.Events;

[Serializable]
public class EventAlreadyExistException : Exception
{
    public EventAlreadyExistException(string name, long chatId) : base($"Event \"{name}\" in chat \"{chatId}\" already exist")
    {
        
    }
}