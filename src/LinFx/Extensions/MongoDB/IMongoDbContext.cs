using MongoDB.Driver;

namespace LinFx.Extensions.MongoDB
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }

        IMongoCollection<T> Collection<T>();
    }
}
