using LinFx;
using LinFx.Extensions.EventBus;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventBusServiceCollectionExtensions
    {
        //public static ILinFxBuilder AddEventBus(this ILinFxBuilder builder, Action<EventBusOptions> optionsAction)
        //{
        //    Check.NotNull(builder, nameof(builder));
        //    Check.NotNull(optionsAction, nameof(optionsAction));

        //    var options = new EventBusOptions();
        //    optionsAction?.Invoke(options);
        //    options.ConfigureEventBus?.Invoke(builder, new EventBusOptionsBuilder(options));

        //    builder.Services.Configure(optionsAction);
        //    builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

        //    return builder;
        //}

        public static ILinFxBuilder AddEventBus(this ILinFxBuilder builder, Action<IEventBusBuilder> configure)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(configure, nameof(configure));

            builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            configure(new EventBusBuilder(builder));
            return builder;
        }
    }
}