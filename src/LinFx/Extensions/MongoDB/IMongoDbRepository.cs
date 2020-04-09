using MongoDB.Driver;

namespace LinFx.Extensions.MongoDB
{
    public interface IMongoDbRepository<TEntity>
    {
        IMongoDatabase Database { get; }

        IMongoCollection<TEntity> Collection { get; }
    }
}
