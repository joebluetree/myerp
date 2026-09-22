namespace MyErp.Entities.Abstractions;

/// <summary>
/// Entities that are flagged as deleted rather than removed from the table.
/// </summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }

    DateTime? DeletedAtUtc { get; set; }

    string? DeletedBy { get; set; }
}
