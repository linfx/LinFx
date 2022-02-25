using MongoDB.Driver;

namespace LinFx.Extensions.MongoDB;

public interface IMongoDbContext
{
    IMongoClient Client { get; }

    IMongoDatabase Database { get; }

    IMongoCollection<T> Collection<T>();

    IClientSessionHandle SessionHandle { get; }
}
