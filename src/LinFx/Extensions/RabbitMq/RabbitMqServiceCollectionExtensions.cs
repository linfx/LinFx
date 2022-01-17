using LinFx;
using LinFx.Extensions.RabbitMq;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class RabbitMqServiceCollectionExtensions
{
    public static LinFxBuilder AddRabbitMq(this LinFxBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Services
            .AddSingleton<IConnectionPool, ConnectionPool>()
            .AddSingleton<IChannelPool, ChannelPool>()
            .AddSingleton<IRabbitMqMessageConsumerFactory, RabbitMqMessageConsumerFactory>()
            .AddSingleton<IRabbitMqSerializer, RabbitMqSerializer>()
            .AddTransient<RabbitMqMessageConsumer>();

        return builder;
    }

    public static LinFxBuilder AddRabbitMq(this LinFxBuilder builder, Action<RabbitMqOptions> optionsAction)
    {
        Check.NotNull(builder, nameof(builder));
        Check.NotNull(optionsAction, nameof(optionsAction));

        var options = new RabbitMqOptions();
        optionsAction?.Invoke(options);
        builder.Services.Configure(optionsAction);

        return builder.AddRabbitMq();
    }
}
