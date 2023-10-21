namespace TelegramBot.Partitions.Shared.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    protected Entity()
    {
        
    }
    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }

    public bool Equals(Entity? other)
    {
        return other is not null && Id == other.Id;
    }
}