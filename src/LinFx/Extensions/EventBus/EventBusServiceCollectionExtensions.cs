using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.EventBus
{
    public static class EventBusServiceCollectionExtensions
    {
        public static ILinFxBuilder AddEventBus(this ILinFxBuilder builder, Action<EventBusOptions> configAction)
        {
            var options = new EventBusOptions();
            configAction?.Invoke(options);
            builder.Services.AddSingleton<IEventBus>(new EventBus(builder.Services));
            return builder;
        }
    }
}