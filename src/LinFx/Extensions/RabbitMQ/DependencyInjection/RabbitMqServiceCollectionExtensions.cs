using LinFx;
using LinFx.Extensions.RabbitMQ;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMqServiceCollectionExtensions
    {
        public static ILinFxBuilder AddRabbitMQ(this ILinFxBuilder builder, Action<RabbitMqOptions> optionsAction)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(optionsAction, nameof(optionsAction));

            var options = new RabbitMqOptions();
            optionsAction?.Invoke(options);
            builder.Services.Configure(optionsAction);

            builder.Services.AddSingleton(sp =>
            {
                return new ConnectionFactoryWrapper(new ConnectionConfiguration
                {
                    Host = options.Host,
                    UserName = options.UserName,
                    Password = options.Password,
                });
            });
            builder.Services.AddSingleton<IConnectionPool, DefaultConnectionPool>();
            builder.Services.AddSingleton<IChannelPool, DefaultChannelPool>();
            builder.Services.AddSingleton<IConsumerFactory, ConsumerFactory>();
            builder.Services.AddSingleton<IRabbitMqSerializer, DefaultRabbitMqSerializer>();
            builder.Services.AddTransient<Consumer>();

            return builder;
        }
    }
}
