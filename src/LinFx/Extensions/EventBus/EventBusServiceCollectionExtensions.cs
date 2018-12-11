using System;
using LinFx;
using LinFx.Extensions.EventBus;

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

            options.ConfigureEventBus?.Invoke(builder, new EventBusOptionsBuilder(options));

            builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return builder;
        }
    }
}