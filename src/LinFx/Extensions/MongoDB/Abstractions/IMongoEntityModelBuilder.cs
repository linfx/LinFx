using MongoDB.Bson.Serialization;
using System;

namespace LinFx.Extensions.MongoDB
{
    public interface IMongoEntityModelBuilder<TEntity>
    {
        Type EntityType { get; }

        string CollectionName { get; set; }

        BsonClassMap<TEntity> BsonMap { get; }
    }

    public interface IMongoEntityModelBuilder
    {
        Type EntityType { get; }

        string CollectionName { get; set; }

        BsonClassMap BsonMap { get; }
    }
}