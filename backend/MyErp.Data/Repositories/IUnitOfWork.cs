namespace MyErp.Data.Repositories;

/// <summary>
/// Commits the work staged on the repositories of the current request, and lets a
/// service wrap several writes in one database transaction when it needs to.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IAsyncDisposable> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
