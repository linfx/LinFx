using JetBrains.Annotations;
using LinFx.Domain.Entities;
using LinFx.Extensions.Data;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Threading;
using LinFx.Extensions.Uow;
using LinFx.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace LinFx.Domain.Repositories;

public abstract class BasicRepositoryBase<TEntity> : IBasicRepository<TEntity>, IServiceProviderAccessor
    where TEntity : class, IEntity
{
    [Autowired]
    public ILazyServiceProvider LazyServiceProvider { get; set; }

    [Autowired]
    public IServiceProvider ServiceProvider { get; set; }

    /// <summary>
    /// 数据过滤
    /// </summary>
    public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

    /// <summary>
    /// 当前租户
    /// </summary>
    public ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    /// <summary>
    /// 异步查询
    /// </summary>
    public IAsyncQueryableExecuter AsyncExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

    /// <summary>
    /// 工作单元管理器
    /// </summary>
    public IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

    public ICancellationTokenProvider CancellationTokenProvider => LazyServiceProvider.LazyGetService<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance);

    public BasicRepositoryBase() { }

    public BasicRepositoryBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        LazyServiceProvider = serviceProvider.GetRequiredService<ILazyServiceProvider>();
    }

    public abstract ValueTask<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await InsertAsync(entity, cancellationToken: cancellationToken);
        }

        if (autoSave)
            await SaveChangesAsync(cancellationToken);
    }

    protected virtual Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        if (UnitOfWorkManager?.Current != null)
            return UnitOfWorkManager.Current.SaveChangesAsync(cancellationToken);

        return Task.CompletedTask;
    }

    public abstract ValueTask<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await UpdateAsync(entity, cancellationToken: cancellationToken);
        }

        if (autoSave)
            await SaveChangesAsync(cancellationToken);
    }

    public abstract Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await DeleteAsync(entity, cancellationToken: cancellationToken);
        }

        if (autoSave)
            await SaveChangesAsync(cancellationToken);
    }

    public abstract ValueTask<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

    public abstract ValueTask<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default);

    public abstract ValueTask<long> GetCountAsync(CancellationToken cancellationToken = default);

    public abstract ValueTask<List<TEntity>> GetPagedListAsync(int page, int limit, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default);

    protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default) => CancellationTokenProvider.FallbackToProvider(preferredValue);
}

public abstract class BasicRepositoryBase<TEntity, TKey> : BasicRepositoryBase<TEntity>, IBasicRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected BasicRepositoryBase() { }

    protected BasicRepositoryBase(IServiceProvider serviceProvider)
        : base(serviceProvider)
    { }

    public virtual async ValueTask<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, includeDetails, cancellationToken);
        if (entity == null)
            throw new EntityNotFoundException(typeof(TEntity), id);

        return entity;
    }

    public abstract ValueTask<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

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
