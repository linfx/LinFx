using LinFx.Extensions.Modularity;
using LinFx.Extensions.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Kafka;

[DependsOn(
    //typeof(AbpJsonModule),
    typeof(ThreadingModule)
)]
public class KafkaModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        var configuration = services.GetConfiguration();
        Configure<KafkaOptions>(configuration.GetSection("Kafka"));
    }

    //public override void OnApplicationShutdown(ApplicationShutdownContext context)
    //{
    //    context.ServiceProvider
    //        .GetRequiredService<IConsumerPool>()
    //        .Dispose();

    //    context.ServiceProvider
    //        .GetRequiredService<IProducerPool>()
    //        .Dispose();
    //}
}
