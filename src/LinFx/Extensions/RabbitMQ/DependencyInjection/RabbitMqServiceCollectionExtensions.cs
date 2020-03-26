using LinFx;
using LinFx.Extensions.RabbitMq;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMqServiceCollectionExtensions
    {
        public static LinFxBuilder AddRabbitMq(this LinFxBuilder builder, Action<RabbitMqOptions> optionsAction)
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

            builder.Services
                .AddSingleton<IConnectionPool, DefaultConnectionPool>()
                .AddSingleton<IChannelPool, DefaultChannelPool>()
                .AddSingleton<IConsumerFactory, RabbitMqConsumerFactory>()
                .AddSingleton<IRabbitMqSerializer, DefaultRabbitMqSerializer>()
                .AddTransient<RabbitMqConsumer>();

            return builder;
        }

        public static LinFxBuilder AddRabbitMqPersistentConnection(this LinFxBuilder builder, Action<RabbitMqOptions> optionsAction)
        {
            var options = new RabbitMqOptions();
            optionsAction?.Invoke(options);

            builder.Services.AddSingleton((Func<IServiceProvider, IRabbitMqPersistentConnection>)(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();
                var factory = new ConnectionFactory
                {
                    HostName = options.Host,
                    UserName = options.UserName,
                    Password = options.Password,
                };
                return new DefaultRabbitMqPersistentConnection(factory, logger);
            }));

            return builder;
        }
    }
}
