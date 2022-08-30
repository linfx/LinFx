using LinFx.Extensions.Kafka;
using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.EventBus.Kafka;

[DependsOn(
    typeof(EventBusModule),
    typeof(KafkaModule)
)]
public class EventBusKafkaModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        var configuration = services.GetConfiguration();
        Configure<KafkaEventBusOptions>(configuration.GetSection("Kafka:EventBus"));
    }

    //public override void OnApplicationInitialization(ApplicationInitializationContext context)
    //{
    //    context
    //        .ServiceProvider
    //        .GetRequiredService<KafkaDistributedEventBus>()
    //        .Initialize();
    //}
}
