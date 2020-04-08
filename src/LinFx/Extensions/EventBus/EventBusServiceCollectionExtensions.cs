using LinFx.Extensions.EventBus;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventBusServiceCollectionExtensions
    {
        public static LinFxBuilder AddEventBus(this LinFxBuilder builder, Action<EventBusOptionsBuilder> optionsAction)
        {
            var options = new EventBusOptions();
            var optionsBuilder = new EventBusOptionsBuilder(builder, options);
            optionsAction?.Invoke(optionsBuilder);

            builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return builder;
        }
    }
}