using LinFx.Domain.Entities;
using System.Linq.Expressions;

namespace LinFx.Domain.Repositories;

public interface ISupportsExplicitLoading<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    Task EnsureCollectionLoadedAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken)
        where TProperty : class;

    Task EnsurePropertyLoadedAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        CancellationToken cancellationToken)
        where TProperty : class;
}
