using MongoDB.Driver;

namespace LinFx.Data.MongoDB
{
    /// <summary>
    /// Defines interface to obtain a <see cref="MongoDatabase"/> object.
    /// </summary>
    public interface IMongoDatabaseProvider
    {
        MongoDatabaseBase Database { get; set; }
    }
}