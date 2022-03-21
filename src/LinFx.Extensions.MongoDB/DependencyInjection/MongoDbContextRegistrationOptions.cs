using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.MongoDB.DependencyInjection;

public class MongoDbContextRegistrationOptions : CommonDbContextRegistrationOptions, IMongoDbContextRegistrationOptionsBuilder
{
    public MongoDbContextRegistrationOptions(Type originalDbContextType, IServiceCollection services)
        : base(originalDbContextType, services)
    {
    }
}
