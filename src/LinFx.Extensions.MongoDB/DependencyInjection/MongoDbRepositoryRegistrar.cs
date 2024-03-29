using LinFx.Domain.Repositories;
using LinFx.Domain.Repositories.MongoDB;

namespace LinFx.Extensions.MongoDB.DependencyInjection;

public class MongoDbRepositoryRegistrar : RepositoryRegistrarBase<MongoDbContextRegistrationOptions>
{
    public MongoDbRepositoryRegistrar(MongoDbContextRegistrationOptions options)
        : base(options)
    {
    }

    protected override IEnumerable<Type> GetEntityTypes(Type dbContextType)
    {
        return MongoDbContextHelper.GetEntityTypes(dbContextType);
    }

    protected override Type GetRepositoryType(Type dbContextType, Type entityType)
    {
        return typeof(MongoDbRepository<,>).MakeGenericType(dbContextType, entityType);
    }

    protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType)
    {
        return typeof(MongoDbRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);
    }
}
