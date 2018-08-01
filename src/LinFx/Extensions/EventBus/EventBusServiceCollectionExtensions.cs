using LinFx.Extensions.EventBus;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventBusServiceCollectionExtensions
    {
        public static ILinFxBuilder AddEventBus(this ILinFxBuilder builder, Action<EventBusOptions> configAction)
        {
            var options = new EventBusOptions();
            configAction?.Invoke(options);
            builder.Services.AddSingleton<IEventBus, EventBus>();
            return builder;
        }
    }
}