namespace MyErp.Common.Interfaces;

/// <summary>
/// The user behind the current request. Implemented by the host, consumed by modules.
/// </summary>
public interface ICurrentUser
{
    string? UserId { get; }

    string? UserName { get; }

    bool IsAuthenticated { get; }
}
