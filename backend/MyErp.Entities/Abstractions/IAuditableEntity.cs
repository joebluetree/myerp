namespace MyErp.Entities.Abstractions;

/// <summary>
/// Audit columns stamped automatically by the DbContext on save.
/// </summary>
public interface IAuditableEntity
{
    DateTime CreatedAtUtc { get; set; }

    string? CreatedBy { get; set; }

    DateTime? ModifiedAtUtc { get; set; }

    string? ModifiedBy { get; set; }
}
