using LinFx.Extensions.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.EventBus.RabbitMQ
{
    public static class EventBusBuilderExtensions
    {
        public static IEventBusBuilder UseRabbitMQ(this IEventBusBuilder builder, Action<RabbitMqOptions> optionsAction)
        {
            Check.NotNull(optionsAction, nameof(optionsAction));

            builder.Fx.AddRabbitMQ(optionsAction);
            builder.Fx.Services.AddSingleton<IEventBus, RabbitMqDistributedEventBus>();

            return builder;
        }
    }
}
