using MongoDB.Bson.Serialization;

namespace LinFx.Extensions.MongoDB
{
    public interface IHasBsonClassMap
    {
        BsonClassMap GetMap();
    }
}