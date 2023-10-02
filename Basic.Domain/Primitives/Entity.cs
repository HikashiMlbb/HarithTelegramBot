namespace Basic.Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    protected Entity(Guid id)
    {
        Id = id;
    }

    private Guid Id { get; }

    public bool Equals(Entity? other)
    {
        return other is not null && Id == other.Id;
    }
}