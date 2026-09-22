namespace MyErp.Entities.Abstractions;

/// <summary>
/// Default base class for persisted entities: a long surrogate key plus audit columns.
/// </summary>
public abstract class BaseEntity : BaseEntity<long>
{
}

public abstract class BaseEntity<TKey> : IEntity<TKey>, IAuditableEntity
{
    public TKey Id { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAtUtc { get; set; }

    public string? ModifiedBy { get; set; }
}
