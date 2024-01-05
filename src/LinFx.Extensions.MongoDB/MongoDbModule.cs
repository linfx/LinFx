using LinFx.Domain.Repositories.MongoDB;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.MongoDB.DependencyInjection;
using LinFx.Extensions.Uow.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LinFx.Extensions.MongoDB;

//[DependsOn(typeof(DomainModule))]
public class MongoDbModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddConventionalRegistrar(new MongoDbConventionalRegistrar());

        services.TryAddTransient(typeof(IMongoDbContextProvider<>), typeof(UnitOfWorkMongoDbContextProvider<>));
        services.TryAddTransient(typeof(IMongoDbRepositoryFilterer<>), typeof(MongoDbRepositoryFilterer<>));
        services.TryAddTransient(typeof(IMongoDbRepositoryFilterer<,>), typeof(MongoDbRepositoryFilterer<,>));

        //context.Services.AddTransient(
        //    typeof(IMongoDbContextEventOutbox<>),
        //    typeof(MongoDbContextEventOutbox<>)
        //);

        //context.Services.AddTransient(
        //    typeof(IMongoDbContextEventInbox<>),
        //    typeof(MongoDbContextEventInbox<>)
        //);
    }
}
