using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.MongoDB.DependencyInjection;

public class MongoDbConventionalRegistrar : DefaultConventionalRegistrar
{
    protected override bool IsConventionalRegistrationDisabled(Type type)
    {
        return !typeof(IMongoDbContext).IsAssignableFrom(type) || type == typeof(MongoDbContext) || base.IsConventionalRegistrationDisabled(type);
    }

    protected override List<Type> GetExposedServiceTypes(Type type)
    {
        return new List<Type>()
            {
                typeof(IMongoDbContext)
            };
    }

    protected override ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
    {
        return ServiceLifetime.Transient;
    }
}
