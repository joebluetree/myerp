namespace MyErp.Common.Results;

/// <summary>
/// Stable error codes shared by every module so clients can branch on them.
/// </summary>
public static class ErrorCodes
{
    public const string NotFound = "not_found";
    public const string Validation = "validation_failed";
    public const string Conflict = "conflict";
    public const string BusinessRule = "business_rule_violated";
    public const string Unauthorized = "unauthorized";
    public const string Forbidden = "forbidden";
    public const string Unexpected = "unexpected_error";
}
