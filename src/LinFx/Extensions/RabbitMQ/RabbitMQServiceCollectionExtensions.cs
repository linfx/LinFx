using LinFx;
using LinFx.Extensions.RabbitMQ;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQServiceCollectionExtensions
    {
        public static ILinFxBuilder AddRabbitMQ(this ILinFxBuilder builder, Action<RabbitMQOptions> optionsAction)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(optionsAction, nameof(optionsAction));

            var options = new RabbitMQOptions();
            optionsAction?.Invoke(options);

            builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory
                {
                    UserName = options.UserName,
                    Password = options.Password,
                    HostName = options.Host,
                };
                return new DefaultRabbitMQPersistentConnection(factory, logger);
            });

            return builder;
        }
    }
}