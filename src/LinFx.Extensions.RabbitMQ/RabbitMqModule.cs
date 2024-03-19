using LinFx.Extensions.Modularity;
using LinFx.Extensions.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.RabbitMQ;

[DependsOn(
    typeof(ThreadingModule)
)]
public class RabbitMqModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        var configuration = services.GetConfiguration();
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMQ"));
        services.Configure<RabbitMqOptions>(options =>
        {
            foreach (var connectionFactory in options.Connections.Values)
            {
                connectionFactory.DispatchConsumersAsync = true;
            }
        });

        //    builder.Services
        //.AddSingleton<IConnectionPool, ConnectionPool>()
        //.AddSingleton<IChannelPool, ChannelPool>()
        //.AddSingleton<IRabbitMqMessageConsumerFactory, RabbitMqMessageConsumerFactory>()
        //.AddSingleton<IRabbitMqSerializer, RabbitMqSerializer>()
        //.AddTransient<RabbitMqMessageConsumer>();
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
