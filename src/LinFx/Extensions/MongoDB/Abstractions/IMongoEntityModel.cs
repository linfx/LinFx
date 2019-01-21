using System;

namespace LinFx.Extensions.MongoDB
{
    public interface IMongoEntityModel
    {
        Type EntityType { get; }

        string CollectionName { get; }
    }
}
