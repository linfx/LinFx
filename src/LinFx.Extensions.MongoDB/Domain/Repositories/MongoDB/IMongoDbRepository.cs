using LinFx.Domain.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LinFx.Domain.Repositories.MongoDB;

public interface IMongoDbRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<IMongoDatabase> GetDatabaseAsync(CancellationToken cancellationToken = default);

    Task<IMongoCollection<TEntity>> GetCollectionAsync(CancellationToken cancellationToken = default);

    Task<IMongoQueryable<TEntity>> GetMongoQueryableAsync(CancellationToken cancellationToken = default);

    Task<IAggregateFluent<TEntity>> GetAggregateAsync(CancellationToken cancellationToken = default);
}

public interface IMongoDbRepository<TEntity, TKey> : IMongoDbRepository<TEntity>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
}