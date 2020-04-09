using LinFx;
using LinFx.Extensions.EventStores;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventStoreServiceCollectionExtensions
    {
        public static LinFxBuilder AddEventStores(
            [NotNull] this LinFxBuilder builder,
            [NotNull] Action<EventStoreOptionsBuilder> optionsAction) 
            => AddEventStores(
                builder,
                optionsAction == null
                    ? (Action<IServiceProvider, EventStoreOptionsBuilder>)null
                    : (p, b) => optionsAction.Invoke(b));


        public static LinFxBuilder AddEventStores(
            [NotNull] this LinFxBuilder builder, 
            [NotNull] Action<IServiceProvider, EventStoreOptionsBuilder> optionsAction,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(optionsAction, nameof(optionsAction));

            AddCoreServices(builder.Services, optionsAction, optionsLifetime);

            builder.Services.AddDbContext<EventStoreContext>();
            builder.Services.AddTransient<IEventStore, EventStoreManager>();

            return builder;
        }

        private static void AddCoreServices(
            IServiceCollection serviceCollection,
            Action<IServiceProvider, EventStoreOptionsBuilder> optionsAction,
            ServiceLifetime optionsLifetime)
        {
            serviceCollection.TryAdd(
                new ServiceDescriptor(
                    typeof(EventStoreOptions),
                    p => EventStoreOptionsFactory(p, optionsAction),
                    optionsLifetime));
        }

        private static EventStoreOptions EventStoreOptionsFactory(
            [NotNull] IServiceProvider applicationServiceProvider,
            [CanBeNull] Action<IServiceProvider, EventStoreOptionsBuilder> optionsAction)
        {
            var builder = new EventStoreOptionsBuilder(new EventStoreOptions());
            optionsAction?.Invoke(applicationServiceProvider, builder);
            return builder.Options;
        }
    }
}