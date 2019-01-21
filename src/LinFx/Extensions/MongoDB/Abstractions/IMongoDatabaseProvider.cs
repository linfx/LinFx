using MongoDB.Driver;

namespace LinFx.Extensions.MongoDB
{
    /// <summary>
    /// Defines interface to obtain a <see cref="MongoDatabaseBase"/> object.
    /// </summary>
    public interface IMongoDatabaseProvider
    {
        MongoDatabaseBase Database { get; set; }
    }
}