namespace MyErp.Common.Results;

/// <summary>
/// A machine readable failure code paired with a human readable description.
/// </summary>
public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error NotFound(string description) => new(ErrorCodes.NotFound, description);

    public static Error Validation(string description) => new(ErrorCodes.Validation, description);

    public static Error Conflict(string description) => new(ErrorCodes.Conflict, description);

    public static Error BusinessRule(string description) => new(ErrorCodes.BusinessRule, description);
}
