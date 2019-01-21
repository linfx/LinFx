using System;
using System.Collections.Generic;

namespace LinFx.Extensions.MongoDB
{
    public interface IMongoModelBuilder
    {
        void Entity<TEntity>(Action<IMongoEntityModelBuilder<TEntity>> buildAction = null);

        void Entity([NotNull] Type entityType, Action<IMongoEntityModelBuilder> buildAction = null);

        IReadOnlyList<IMongoEntityModel> GetEntities();
    }
}