using System.Linq.Expressions;
using MyErp.Common.Models;
using MyErp.Entities.Abstractions;

namespace MyErp.Data.Repositories;

/// <summary>
/// Persistence gateway for a single entity type. Module services depend on this
/// rather than on <see cref="Context.AppDbContext"/> directly.
/// </summary>
public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    /// <summary>
    /// Untracked queryable for read paths that need to compose their own projection.
    /// </summary>
    IQueryable<TEntity> Query();

    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TEntity>> PagedListAsync(
        PagedRequest request,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    void Update(TEntity entity);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);
}

/// <summary>
/// Convenience shape for the common case of a <see cref="long"/> primary key.
/// </summary>
public interface IRepository<TEntity> : IRepository<TEntity, long> where TEntity : class, IEntity<long>
{
}
