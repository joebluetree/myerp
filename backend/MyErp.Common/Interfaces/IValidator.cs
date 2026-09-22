using MyErp.Common.Results;

namespace MyErp.Common.Interfaces;

/// <summary>
/// Input validation abstraction, kept free of any specific validation library.
/// </summary>
public interface IValidator<in T>
{
    Task<Result> ValidateAsync(T instance, CancellationToken cancellationToken = default);
}
