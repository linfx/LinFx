using JetBrains.Annotations;

namespace LinFx.Extensions.MongoDB.MongoDB;

public interface IMongoModelBuilder
{
    void Entity<TEntity>(Action<IMongoEntityModelBuilder<TEntity>> buildAction = null);

    void Entity([NotNull] Type entityType, Action<IMongoEntityModelBuilder> buildAction = null);

    IReadOnlyList<IMongoEntityModel> GetEntities();
}
