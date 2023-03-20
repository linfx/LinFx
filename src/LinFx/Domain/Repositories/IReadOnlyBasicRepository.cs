using LinFx.Domain.Entities;

namespace LinFx.Domain.Repositories;

public interface IReadOnlyBasicRepository<TEntity> : IRepository
    where TEntity : class, IEntity
{
    /// <summary>
    /// Gets a list of all the entities.
    /// </summary>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Entity</returns>
    ValueTask<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets total count of all entities.
    /// </summary>
    ValueTask<long> GetCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a page list of all the entities.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="sorting"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<List<TEntity>> GetPagedListAsync(int page, int pageSize, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default);
}

public interface IReadOnlyBasicRepository<TEntity, TKey> : IReadOnlyBasicRepository<TEntity>
    where TEntity : class, IEntity<TKey>
{
    /// <summary>
    /// Gets an entity with given primary key.
    /// </summary>
    /// <param name="id">Primary key of the entity to get</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Entity</returns>
    ValueTask<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity with given primary key or null if not found.
    /// </summary>
    /// <param name="id">Primary key of the entity to get</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Entity or null</returns>
    ValueTask<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);
}
