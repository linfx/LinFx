using LinFx;
using LinFx.Extensions.EventBus;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventBusServiceCollectionExtensions
    {
        //public static ILinFxBuilder AddEventBus(
        //    [NotNull] this ILinFxBuilder builder,
        //    [NotNull] Action<EventBusOptions> optionsAction)
        //{
        //    Check.NotNull(builder, nameof(builder));
        //    Check.NotNull(optionsAction, nameof(optionsAction));

        //    var optionsBuilder = new EventBusOptions();
        //    optionsAction?.Invoke(optionsBuilder);

        //    return builder;
        //}

        public static LinFxBuilder AddEventBus(this LinFxBuilder fx, Action<EventBusOptionsBuilder> optionsAction)
        {
            var options = new EventBusOptions();
            var optionsBuilder = new EventBusOptionsBuilder(fx, options);
            optionsAction?.Invoke(optionsBuilder);

            fx.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return fx;
        }
    }
}