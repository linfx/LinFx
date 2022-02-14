using LinFx.Domain.Entities;
using LinFx.Domain.Repositories;
using LinFx.Extensions.EntityFrameworkCore.DependencyInjection;
using LinFx.Extensions.Guids;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.EntityFrameworkCore.Repositories;

/// <summary>
/// Ef 仓储
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
public class EfCoreRepository<TDbContext, TEntity> : RepositoryBase<TEntity>, IEfCoreRepository<TEntity>
    where TDbContext : IEfDbContext
    where TEntity : class, IEntity
{
    public EfCoreRepository(
        IServiceProvider serviceProvider,
        IDbContextProvider<TDbContext> dbContextProvider) : base(serviceProvider)
    {
        _dbContextProvider = dbContextProvider;
        _entityOptionsLazy = new Lazy<EntityOptions<TEntity>>(() => ServiceProvider
                      .GetRequiredService<IOptions<EntityOptions>>()
                      .Value
                      .GetOrNull<TEntity>() ?? EntityOptions<TEntity>.Empty
        );
    }

    async Task<DbContext> IEfCoreRepository<TEntity>.GetDbContextAsync()
    {
        return await GetDbContextAsync() as DbContext;
    }

    /// <summary>
    /// 获取数据库上下文
    /// </summary>
    /// <returns></returns>
    protected virtual Task<TDbContext> GetDbContextAsync()
    {
        // Multi-tenancy unaware entities should always use the host connection string
        if (!EntityHelper.IsMultiTenant<TEntity>())
        {
            using (CurrentTenant.Change(null))
            {
                return _dbContextProvider.GetDbContextAsync();
            }
        }

        return _dbContextProvider.GetDbContextAsync();
    }

    Task<DbSet<TEntity>> IEfCoreRepository<TEntity>.GetDbSetAsync()
    {
        return GetDbSetAsync();
    }

    protected async Task<DbSet<TEntity>> GetDbSetAsync()
    {
        return (await GetDbContextAsync()).Set<TEntity>();
    }

    protected virtual EntityOptions<TEntity> EntityOptions => _entityOptionsLazy.Value;

    private readonly IDbContextProvider<TDbContext> _dbContextProvider;
    private readonly Lazy<EntityOptions<TEntity>> _entityOptionsLazy;

    public virtual IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

    public IEfCoreBulkOperationProvider BulkOperationProvider => LazyServiceProvider.LazyGetService<IEfCoreBulkOperationProvider>();

    public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        CheckAndSetId(entity);

        var dbContext = await GetDbContextAsync();
        var savedEntity = (await dbContext.Set<TEntity>().AddAsync(entity, GetCancellationToken(cancellationToken))).Entity;
        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }

        return savedEntity;
    }

    public override async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entityArray = entities.ToArray();
        var dbContext = await GetDbContextAsync();
        cancellationToken = GetCancellationToken(cancellationToken);

        foreach (var entity in entityArray)
        {
            CheckAndSetId(entity);
        }

        if (BulkOperationProvider != null)
        {
            await BulkOperationProvider.InsertManyAsync<TDbContext, TEntity>(
                this,
                entityArray,
                autoSave,
                GetCancellationToken(cancellationToken)
            );
            return;
        }

        await dbContext.Set<TEntity>().AddRangeAsync(entityArray, cancellationToken);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        dbContext.Attach(entity);
        var updatedEntity = dbContext.Update(entity).Entity;
        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }
        return updatedEntity;
    }

    public override async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        if (BulkOperationProvider != null)
        {
            await BulkOperationProvider.UpdateManyAsync<TDbContext, TEntity>(
                this,
                entities,
                autoSave,
                GetCancellationToken(cancellationToken)
                );

            return;
        }

        var dbContext = await GetDbContextAsync();
        dbContext.Set<TEntity>().UpdateRange(entities);
        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public override async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        dbContext.Set<TEntity>().Remove(entity);
        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }
    }

    public override async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        if (BulkOperationProvider != null)
        {
            await BulkOperationProvider.DeleteManyAsync<TDbContext, TEntity>(
                this,
                entities,
                autoSave,
                cancellationToken
            );

            return;
        }

        var dbContext = await GetDbContextAsync();
        dbContext.RemoveRange(entities);
        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public override async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await WithDetailsAsync()).ToListAsync(GetCancellationToken(cancellationToken))
            : await (await GetDbSetAsync()).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public override async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await WithDetailsAsync()).Where(predicate).ToListAsync(GetCancellationToken(cancellationToken))
            : await (await GetDbSetAsync()).Where(predicate).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public override async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public override async Task<List<TEntity>> GetPagedListAsync(
        int page,
        int limit,
        string sorting,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var queryable = includeDetails
            ? await WithDetailsAsync()
            : await GetDbSetAsync();

        return await queryable
            .OrderBy(sorting)
            .PageBy(page, limit)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public override async Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default)
    {
        return (await GetDbSetAsync()).AsQueryable();
    }

    protected override async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await (await GetDbContextAsync()).SaveChangesAsync(cancellationToken);
    }

    public override async Task<TEntity> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await WithDetailsAsync())
                .Where(predicate)
                .SingleOrDefaultAsync(GetCancellationToken(cancellationToken))
            : await (await GetDbSetAsync())
                .Where(predicate)
                .SingleOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<TEntity>();

        var entities = await dbSet
            .Where(predicate)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(entities, autoSave, cancellationToken);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }
    }

    public virtual async Task EnsureCollectionLoadedAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken = default)
        where TProperty : class
    {
        await (await GetDbContextAsync())
            .Entry(entity)
            .Collection(propertyExpression)
            .LoadAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task EnsurePropertyLoadedAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        CancellationToken cancellationToken = default)
        where TProperty : class
    {
        await (await GetDbContextAsync())
            .Entry(entity)
            .Reference(propertyExpression)
            .LoadAsync(GetCancellationToken(cancellationToken));
    }

    public override async Task<IQueryable<TEntity>> WithDetailsAsync(CancellationToken token)
    {
        if (EntityOptions.DefaultWithDetailsFunc == null)
            return await base.WithDetailsAsync(token);

        return EntityOptions.DefaultWithDetailsFunc(await GetQueryableAsync());
    }

    public override async Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return IncludeDetails(
            await GetQueryableAsync(),
            propertySelectors
        );
    }

    private static IQueryable<TEntity> IncludeDetails(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, object>>[] propertySelectors)
    {
        if (!propertySelectors.IsNullOrEmpty())
        {
            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }
        }

        return query;
    }

    protected virtual void CheckAndSetId(TEntity entity)
    {
        if (entity is IEntity<Guid> entityWithGuidId)
        {
            TrySetGuidId(entityWithGuidId);
        }
    }

    protected virtual void TrySetGuidId(IEntity<Guid> entity)
    {
        if (entity.Id != default)
            return;

        EntityHelper.TrySetId(entity, IDUtils.NewIdString(), true);
    }
}

public class EfCoreRepository<TDbContext, TEntity, TKey> : EfCoreRepository<TDbContext, TEntity>, IEfCoreRepository<TEntity, TKey>, ISupportsExplicitLoading<TEntity, TKey>
    where TDbContext : IEfDbContext
    where TEntity : class, IEntity<TKey>
{
    public EfCoreRepository(IServiceProvider serviceProvider, IDbContextProvider<TDbContext> dbContextProvider)
        : base(serviceProvider, dbContextProvider)
    {
    }

    public virtual async Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, includeDetails, GetCancellationToken(cancellationToken));
        if (entity == null)
            throw new EntityNotFoundException(typeof(TEntity), id);

        return entity;
    }

    public virtual async Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await WithDetailsAsync()).OrderBy(e => e.Id).FirstOrDefaultAsync(e => e.Id.Equals(id), GetCancellationToken(cancellationToken))
            : await (await GetDbSetAsync()).FindAsync(new object[] { id }, GetCancellationToken(cancellationToken));
    }

    public virtual async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
            return;

        await DeleteAsync(entity, autoSave, cancellationToken);
    }

    public virtual async Task DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var entities = await (await GetDbSetAsync()).Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
        await DeleteManyAsync(entities, autoSave, cancellationToken);
    }
}
