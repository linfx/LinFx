using LinFx.Application;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.EventBus.RabbitMQ;

[DependsOn(
    typeof(EventBusModule),
    typeof(RabbitMqModule))]
public class EventBusRabbitMqModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        var configuration = services.GetConfiguration();
        services.Configure<RabbitMqEventBusOptions>(configuration.GetSection("RabbitMQ:EventBus"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        context
            .ServiceProvider
            .GetRequiredService<RabbitMqDistributedEventBus>()
            .Initialize();
    }
}
