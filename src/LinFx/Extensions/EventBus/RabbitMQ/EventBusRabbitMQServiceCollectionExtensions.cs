using LinFx.Extensions.EventBus.Abstractions;
using LinFx.Extensions.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace LinFx.Extensions.EventBus.RabbitMQ
{
    public static class EventBusRabbitMQServiceCollectionExtensions
    {
        public static ILinFxBuilder AddEventBusRabbitMQ(this ILinFxBuilder builder, Action<EventBusOptions> optionsAction)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(optionsAction, nameof(optionsAction));

            //Options and extension service
            var options = new EventBusOptions();
            optionsAction?.Invoke(options);

            builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iServiceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMQ(logger, rabbitMQPersistentConnection, eventBusSubcriptionsManager, iServiceScopeFactory, options);
            });

            return builder;
        }
    }
}
