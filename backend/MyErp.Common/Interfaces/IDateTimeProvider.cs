namespace MyErp.Common.Interfaces;

/// <summary>
/// Clock abstraction so time dependent rules stay testable.
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
