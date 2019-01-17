using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.EventBus.RabbitMQ
{
    public static class EventBusBuilderExtensions
    {
        public static IEventBusBuilder UseRabbitMQ(this IEventBusBuilder builder, Action<RabbitMqDistributedEventBusOptions> optionsAction)
        {
            Check.NotNull(optionsAction, nameof(optionsAction));

            builder.Fx.Services.Configure(optionsAction);

            builder.Fx.AddRabbitMQ(options=>
            {
            });
            builder.Fx.Services.AddSingleton<IEventBus, RabbitMqDistributedEventBus>();

            return builder;
        }
    }
}
