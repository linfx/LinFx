using MongoDB.Driver;

namespace LinFx.Extensions.Mongo
{
    /// <summary>
    /// Defines interface to obtain a <see cref="MongoDatabaseBase"/> object.
    /// </summary>
    public interface IMongoDatabaseProvider
    {
        MongoDatabaseBase Database { get; set; }
    }
}