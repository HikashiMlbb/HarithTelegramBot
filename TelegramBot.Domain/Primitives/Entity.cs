namespace TelegramBot.Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    private Guid Id { get; set; }

    protected Entity(Guid id)
    {
        Id = id;
    }

    public bool Equals(Entity? other)
    {
        return (other is not null) && (Id == other.Id);
    }
}