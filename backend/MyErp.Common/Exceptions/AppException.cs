using MyErp.Common.Results;

namespace MyErp.Common.Exceptions;

/// <summary>
/// Base type for failures the application raises deliberately and knows how to
/// translate into an HTTP response.
/// </summary>
public abstract class AppException : Exception
{
    protected AppException(string code, string message)
        : base(message)
    {
        Code = code;
    }

    public string Code { get; }
}

public sealed class NotFoundException(string message) : AppException(ErrorCodes.NotFound, message)
{
    public static NotFoundException For(string entity, object key) =>
        new($"{entity} '{key}' was not found.");
}

public sealed class ConflictException(string message) : AppException(ErrorCodes.Conflict, message);

public sealed class BusinessRuleException(string message) : AppException(ErrorCodes.BusinessRule, message);

public sealed class ValidationException : AppException
{
    public ValidationException(string message)
        : base(ErrorCodes.Validation, message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IDictionary<string, string[]> errors)
        : base(ErrorCodes.Validation, "One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public IDictionary<string, string[]> Errors { get; }
}
