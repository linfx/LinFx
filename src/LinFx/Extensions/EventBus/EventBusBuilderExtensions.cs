using System;

namespace LinFx.Extensions.EventBus
{
    public static class EventBusBuilderExtensions
    {
        public static IEventBusBuilder Configure(this IEventBusBuilder builder, Action<EventBusOptions> configureDelegate)
        {
            return builder;
        }
    }
}
