using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using LinFx.Extensions.RabbitMQ;
using RabbitMQ.Client;

namespace LinFx.Extensions.EventBus.RabbitMQ
{
    public static class EventBusOptionsExtensions
    {
        [Obsolete]
        public static EventBusOptionsBuilder UseRabbitMQ(this EventBusOptionsBuilder optionsBuilder, ILinFxBuilder builder, Action<EventBusRabbitMqOptions> optionsAction)
        {
            Check.NotNull(optionsAction, nameof(optionsAction));

            var options = new EventBusRabbitMqOptions();
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

            builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iServiceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                return new EventBusRabbitMQ(logger, rabbitMQPersistentConnection, eventBusSubcriptionsManager, iServiceScopeFactory, optionsBuilder.Options);
            });

            return optionsBuilder;
        }
    }
}
