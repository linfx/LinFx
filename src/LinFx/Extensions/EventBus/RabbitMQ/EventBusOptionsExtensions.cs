using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using LinFx.Extensions.EventBus.Abstractions;
using LinFx.Extensions.RabbitMQ;

namespace LinFx.Extensions.EventBus.RabbitMQ
{
    public static class EventBusOptionsExtensions
    {
        public static EventBusOptionsBuilder UseRabbitMQ(this EventBusOptionsBuilder optionsBuilder, ILinFxBuilder fx, Action<RabbitMQOptions> optionsAction)
        {
            Check.NotNull(optionsAction, nameof(optionsAction));

            var options = new RabbitMQOptions();
            optionsAction?.Invoke(options);

            fx.AddRabbitMQ(x =>
            {
                x.Host = options.Host;
                x.UserName = options.UserName;
                x.Password = options.Password;
            });

            fx.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
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
