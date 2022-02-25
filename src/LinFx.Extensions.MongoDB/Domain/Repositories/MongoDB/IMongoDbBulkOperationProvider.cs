using LinFx.Domain.Entities;
using MongoDB.Driver;

namespace LinFx.Domain.Repositories.MongoDB;

public interface IMongoDbBulkOperationProvider
{
    Task InsertManyAsync<TEntity>(
       IMongoDbRepository<TEntity> repository,
       IEnumerable<TEntity> entities,
        IClientSessionHandle sessionHandle,
       bool autoSave,
       CancellationToken cancellationToken
   )
       where TEntity : class, IEntity;

    Task UpdateManyAsync<TEntity>(
        IMongoDbRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        IClientSessionHandle sessionHandle,
        bool autoSave,
        CancellationToken cancellationToken
    )
        where TEntity : class, IEntity;

    Task DeleteManyAsync<TEntity>(
        IMongoDbRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        IClientSessionHandle sessionHandle,
        bool autoSave,
        CancellationToken cancellationToken
    )
        where TEntity : class, IEntity;
}
