using System;

namespace Linfx.Extensions.MongoDB
{
    public interface IMongoEntityModel
    {
        Type EntityType { get; }

        string CollectionName { get; }
    }
}
