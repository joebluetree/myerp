using Microsoft.EntityFrameworkCore.Storage;
using MyErp.Data.Context;

namespace MyErp.Data.Repositories;

/// <inheritdoc cref="IUnitOfWork" />
public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork, IAsyncDisposable
{
    private IDbContextTransaction? _transaction;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);

    public async Task<IAsyncDisposable> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            throw new InvalidOperationException("A transaction is already in progress for this unit of work.");
        }

        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        return this;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("There is no transaction to commit.");
        }

        await context.SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);
        await DisposeTransactionAsync();
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            return;
        }

        await _transaction.RollbackAsync(cancellationToken);
        await DisposeTransactionAsync();
    }

    public async ValueTask DisposeAsync()
    {
        // Disposing without an explicit commit rolls the transaction back.
        await DisposeTransactionAsync();
    }

    private async Task DisposeTransactionAsync()
    {
        if (_transaction is null)
        {
            return;
        }

        await _transaction.DisposeAsync();
        _transaction = null;
    }
}
