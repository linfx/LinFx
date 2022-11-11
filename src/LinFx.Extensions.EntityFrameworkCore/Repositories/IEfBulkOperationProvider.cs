using LinFx.Domain.Entities;

namespace LinFx.Extensions.EntityFrameworkCore.Repositories;

public interface IEfBulkOperationProvider
{
    Task InsertManyAsync<TDbContext, TEntity>(
        IEfRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken
    )
        where TDbContext : IEfDbContext
        where TEntity : class, IEntity;


    Task UpdateManyAsync<TDbContext, TEntity>(
        IEfRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken
    )
        where TDbContext : IEfDbContext
        where TEntity : class, IEntity;


    Task DeleteManyAsync<TDbContext, TEntity>(
        IEfRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken
    )
        where TDbContext : IEfDbContext
        where TEntity : class, IEntity;
}
