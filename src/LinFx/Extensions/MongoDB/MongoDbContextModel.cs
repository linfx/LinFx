using System;
using System.Collections.Generic;

namespace LinFx.Extensions.MongoDB
{
    public class MongoDbContextModel
    {
        public IDictionary<Type, IMongoEntityModel> Entities { get; }

        public MongoDbContextModel(IDictionary<Type, IMongoEntityModel> entities)
        {
            Entities = entities;
        }
    }
}
