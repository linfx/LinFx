using LinFx.Extensions.Modularity;
using LinFx.Extensions.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.RabbitMQ;

[DependsOn(
    typeof(ThreadingModule)
)]
public class RabbitMqModule : Module
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<RabbitMqOptions>(configuration.GetSection("RabbitMQ"));
        Configure<RabbitMqOptions>(options =>
        {
            foreach (var connectionFactory in options.Connections.Values)
            {
                connectionFactory.DispatchConsumersAsync = true;
            }
        });
    }

    //public override void OnApplicationShutdown(ApplicationShutdownContext context)
    //{
    //    context.ServiceProvider
    //        .GetRequiredService<IChannelPool>()
    //        .Dispose();

    //    context.ServiceProvider
    //        .GetRequiredService<IConnectionPool>()
    //        .Dispose();
    //}
}
