using LinFx.Domain.Entities;
using LinFx.Linq;
using System.Linq.Expressions;

namespace LinFx.Domain.Repositories;

public interface IReadOnlyRepository<TEntity> : IReadOnlyBasicRepository<TEntity>
    where TEntity : class, IEntity
{
    IAsyncQueryableExecuter AsyncExecuter { get; }

    [Obsolete]
    Task<IQueryable<TEntity>> WithDetailsAsync(CancellationToken cancellationToken = default);

    [Obsolete]
    Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors);

    [Obsolete]
    Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default);

    IQueryable<TEntity> Queryable { get; }

    /// <summary>
    /// Gets a list of entities by the given <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">A condition to filter the entities</param>
    /// <param name="includeDetails">Set true to include details (sub-collections) of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    ValueTask<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default);
}

public interface IReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity>, IReadOnlyBasicRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
}
