using LinFx.Domain.Repositories.MongoDB;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.MongoDB.DependencyInjection;
using LinFx.Extensions.MongoDB.DistributedEvents;
using LinFx.Extensions.Uow.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LinFx.Extensions.MongoDB;

//[DependsOn(typeof(DomainModule))]
public class MongoDbModule : Module
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddConventionalRegistrar(new MongoDbConventionalRegistrar());
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddTransient(
            typeof(IMongoDbContextProvider<>),
            typeof(UnitOfWorkMongoDbContextProvider<>)
        );

        context.Services.TryAddTransient(
            typeof(IMongoDbRepositoryFilterer<>),
            typeof(MongoDbRepositoryFilterer<>)
        );

        context.Services.TryAddTransient(
            typeof(IMongoDbRepositoryFilterer<,>),
            typeof(MongoDbRepositoryFilterer<,>)
        );

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
