using LinFx.Application;
using LinFx.Extensions.EventBus.Distributed;
using LinFx.Extensions.EventBus.Local;
using LinFx.Extensions.Modularity;
using LinFx.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.EventBus;

/// <summary>
/// 事件总线模块
/// </summary>
public class EventBusModule : Module
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AddEventHandlers(context.Services);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        //context.AddBackgroundWorker<OutboxSenderManager>();
        //context.AddBackgroundWorker<InboxProcessManager>();
    }

    private static void AddEventHandlers(IServiceCollection services)
    {
        var localHandlers = new List<Type>();
        var distributedHandlers = new List<Type>();

        services.OnRegistred(context =>
        {
            if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(ILocalEventHandler<>)))
            {
                localHandlers.Add(context.ImplementationType);
            }
            else if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(IDistributedEventHandler<>)))
            {
                distributedHandlers.Add(context.ImplementationType);
            }
        });

        services.Configure<LocalEventBusOptions>(options =>
        {
            options.Handlers.AddIfNotContains(localHandlers);
        });

        services.Configure<DistributedEventBusOptions>(options =>
        {
            options.Handlers.AddIfNotContains(distributedHandlers);
        });
    }
}
