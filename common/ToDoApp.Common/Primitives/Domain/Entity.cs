using TripManager.Common.Primitives.DomainEvents;

namespace TripManager.Common.Primitives.Domain;

public interface IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    void RaiseDomainEvent(IDomainEvent domainEvent);
    void ClearDomainEvents();
}

public abstract class Entity<TId> : IEquatable<Entity<TId>>, IEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// The entity identifier.
    /// </summary>
    public TId Id { get; } = default!;

    /// <summary>
    /// The domain events raised by the entity.
    /// </summary>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    protected Entity()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    protected Entity(TId id)
    {
        Id = id;
    }

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        => left is not null && right is not null && left.Equals(right);

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
        => !(left == right);

    public bool Equals(Entity<TId>? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (Id is null || other.Id is null)
            return false;

        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        if (Id is null || ((Entity<TId>)obj).Id is null)
            return false;

        return Id.Equals(((Entity<TId>)obj).Id);
    }

    public override int GetHashCode()
        => (GetType(), Id).GetHashCode();

    public void RaiseDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}