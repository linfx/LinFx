using LinFx;
using LinFx.Extensions.EventBus;
using LinFx.Extensions.EventBus.RabbitMQ;
using LinFx.Extensions.RabbitMQ;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventBusBuilderExtensions
    {
        public static EventBusOptionsBuilder UseRabbitMQ(
            [NotNull] this EventBusOptionsBuilder optionsBuilder,
            [NotNull] Action<RabbitMqOptions> optionsAction)
        {
            Check.NotNull(optionsBuilder, nameof(optionsBuilder));
            Check.NotNull(optionsAction, nameof(optionsAction));

            optionsBuilder.Fx.AddRabbitMQ(optionsAction);
            optionsBuilder.Fx.Services.AddSingleton<IEventBus, RabbitMqDistributedEventBus>();

            return optionsBuilder;
        }
    }

    public static class EventBusOptionsExtensions
    {
        public static void UseRabbitMQ(
            [NotNull] this EventBusOptions options,
            [NotNull] LinFxBuilder fx,
            [NotNull] Action<RabbitMqOptions> optionsAction)
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(optionsAction, nameof(optionsAction));
            Check.NotNull(fx, nameof(fx));

            fx.AddRabbitMQ(optionsAction);
            fx.Services.AddSingleton<IEventBus, RabbitMqDistributedEventBus>();
        }
    }
}
