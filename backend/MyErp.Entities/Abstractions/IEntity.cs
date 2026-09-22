namespace MyErp.Entities.Abstractions;

/// <summary>
/// Marker for anything the shared DbContext persists.
/// </summary>
public interface IEntity
{
}

/// <summary>
/// An entity identified by a single primary key of type <typeparamref name="TKey"/>.
/// </summary>
public interface IEntity<TKey> : IEntity
{
    TKey Id { get; }
}
