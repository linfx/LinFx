using LinFx.Domain.Abstractions;
using MongoDB.Driver;

namespace linFx.Extensions.MongoDB
{
    public interface IMongoDbRepository<TEntity>
    {
        IMongoDatabase Database { get; }

        IMongoCollection<TEntity> Collection { get; }
    }
}
