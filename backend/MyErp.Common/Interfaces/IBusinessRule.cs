using MyErp.Common.Results;

namespace MyErp.Common.Interfaces;

/// <summary>
/// A single business rule evaluated against <typeparamref name="TContext"/>.
/// Modules put their rule implementations under Features/&lt;Feature&gt;/BusinessRules.
/// </summary>
public interface IBusinessRule<in TContext>
{
    Task<Result> CheckAsync(TContext context, CancellationToken cancellationToken = default);
}
