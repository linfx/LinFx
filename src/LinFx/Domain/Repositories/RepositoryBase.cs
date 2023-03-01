using JetBrains.Annotations;
using LinFx.Domain.Entities;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.Uow;
using System.Linq.Expressions;

namespace LinFx.Domain.Repositories;

public abstract class RepositoryBase<TEntity> : BasicRepositoryBase<TEntity>, IRepository<TEntity>, IUnitOfWorkManagerAccessor
    where TEntity : class, IEntity
{
    public RepositoryBase() { }

    public RepositoryBase(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }

    public virtual Task<IQueryable<TEntity>> WithDetailsAsync(CancellationToken cancellationToken = default) => GetQueryableAsync(cancellationToken);

    public virtual Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors) => GetQueryableAsync();

    public abstract Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default);

    public abstract Task<TEntity> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(predicate, includeDetails, cancellationToken);
        if (entity == null)
            throw new EntityNotFoundException(typeof(TEntity));

        return entity;
    }

    public abstract Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);

    protected virtual TQueryable ApplyDataFilters<TQueryable>(TQueryable query) where TQueryable : IQueryable<TEntity>
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            query = (TQueryable)query.WhereIf(DataFilter.IsEnabled<ISoftDelete>(), e => ((ISoftDelete)e).IsDeleted == false);
        }

        if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
        {
            var tenantId = CurrentTenant.Id;
            query = (TQueryable)query.WhereIf(DataFilter.IsEnabled<IMultiTenant>(), e => ((IMultiTenant)e).TenantId == tenantId);
        }

        return query;
    }
}

public abstract class RepositoryBase<TEntity, TKey> : RepositoryBase<TEntity>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    public RepositoryBase() { }

    protected RepositoryBase(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    public abstract Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

    public abstract Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

    public virtual async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
            return;

        await DeleteAsync(entity, autoSave, cancellationToken);
    }

    public async Task DeleteManyAsync([NotNull] IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        foreach (var id in ids)
        {
            await DeleteAsync(id, cancellationToken: cancellationToken);
        }

        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }
}
