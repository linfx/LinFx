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

        public static LinFxBuilder AddEventBus(
            [NotNull] this LinFxBuilder builder,
            [NotNull] Action<EventBusOptionsBuilder> optionsAction)
        {
            var options = new EventBusOptions();
            var optionsBuilder = new EventBusOptionsBuilder(builder, options);
            optionsAction?.Invoke(optionsBuilder);

            builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return builder;
        }

        //public static ILinFxBuilder AddEventBus(
        //    [NotNull] this ILinFxBuilder builder,
        //    [NotNull] Action<EventBusOptionsBuilder> optionsAction)
        //    => AddEventBus(
        //        builder,
        //        optionsAction == null
        //            ? (Action<IServiceProvider, EventBusOptionsBuilder>)null
        //            : (p, b) => optionsAction.Invoke(b));

        //private static ILinFxBuilder AddEventBus(
        //    [NotNull] this ILinFxBuilder builder,
        //    [NotNull] Action<IServiceProvider, EventBusOptionsBuilder> optionsAction,
        //    ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        //{
        //    Check.NotNull(builder, nameof(builder));
        //    Check.NotNull(optionsAction, nameof(optionsAction));

        //    //AddCoreServices(builder.Services, optionsAction, optionsLifetime);
        //    //builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

        //    return builder;
        //}

        //private static void AddCoreServices(
        //    IServiceCollection serviceCollection,
        //    Action<IServiceProvider, EventBusOptionsBuilder> optionsAction,
        //    ServiceLifetime optionsLifetime)
        //{
        //    serviceCollection.TryAdd(
        //        new ServiceDescriptor(
        //            typeof(EventBusOptions),
        //            p => EventStoreOptionsFactory(p, optionsAction),
        //            optionsLifetime));
        //}

        //private static EventBusOptions EventStoreOptionsFactory(
        //    [NotNull] IServiceProvider applicationServiceProvider,
        //    [CanBeNull] Action<IServiceProvider, EventBusOptionsBuilder> optionsAction)
        //{
        //    var builder = new EventBusOptionsBuilder(new EventBusOptions());
        //    optionsAction?.Invoke(applicationServiceProvider, builder);
        //    return builder.Options;
        //}
    }
}