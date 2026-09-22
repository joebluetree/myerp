using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyErp.Common.Models;
using MyErp.Data.Context;
using MyErp.Entities.Abstractions;

namespace MyErp.Data.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IRepository{TEntity,TKey}"/>.
/// Writes are staged on the change tracker and committed by <see cref="IUnitOfWork"/>.
/// </summary>
public class Repository<TEntity, TKey>(AppDbContext context) : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected AppDbContext Context { get; } = context;

    protected DbSet<TEntity> Set => Context.Set<TEntity>();

    public IQueryable<TEntity> Query() => Set.AsNoTracking();

    public Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default) =>
        Set.FindAsync([id], cancellationToken).AsTask();

    public Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        Set.FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default) =>
        await ApplyFilter(predicate).ToListAsync(cancellationToken);

    public async Task<PagedResult<TEntity>> PagedListAsync(
        PagedRequest request,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter(predicate);

        var totalCount = await query.CountAsync(cancellationToken);
        if (totalCount == 0)
        {
            return PagedResult<TEntity>.Empty(request.Page, request.PageSize);
        }

        var items = await query
            .Skip(request.Skip)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TEntity>(items, totalCount, request.Page, request.PageSize);
    }

    public Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        Set.AsNoTracking().AnyAsync(predicate, cancellationToken);

    public Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default) =>
        ApplyFilter(predicate).CountAsync(cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await Set.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) =>
        await Set.AddRangeAsync(entities, cancellationToken);

    public void Update(TEntity entity) => Set.Update(entity);

    public void Remove(TEntity entity) => Set.Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) => Set.RemoveRange(entities);

    private IQueryable<TEntity> ApplyFilter(Expression<Func<TEntity, bool>>? predicate)
    {
        var query = Set.AsNoTracking();
        return predicate is null ? query : query.Where(predicate);
    }
}

/// <inheritdoc cref="IRepository{TEntity}" />
public class Repository<TEntity>(AppDbContext context)
    : Repository<TEntity, long>(context), IRepository<TEntity>
    where TEntity : class, IEntity<long>
{
}
