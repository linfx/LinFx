using LinFx;
using LinFx.Extensions.EventBus;
using LinFx.Extensions.EventBus.Abstractions;
using LinFx.Extensions.EventBus.RabbitMQ;
using LinFx.Extensions.RabbitMQ;
using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventBusServiceCollectionExtensions
    {
        public static ILinFxBuilder AddEventBus(this ILinFxBuilder builder, Action<EventBusOptions> optionsAction)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(optionsAction, nameof(optionsAction));

            var options = new EventBusOptions();
            optionsAction?.Invoke(options);

            builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                //var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var iServiceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMQ(logger, rabbitMQPersistentConnection, eventBusSubcriptionsManager, iServiceScopeFactory, options);
            });

            return builder;
        }
    }
}