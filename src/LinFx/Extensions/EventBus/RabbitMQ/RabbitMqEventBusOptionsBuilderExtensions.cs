using LinFx;
using LinFx.Extensions.EventBus;
using LinFx.Extensions.EventBus.RabbitMq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventBusBuilderExtensions
    {
        public static EventBusOptionsBuilder UseRabbitMq(
            [NotNull] this EventBusOptionsBuilder optionsBuilder,
            [NotNull] Action<RabbitMqEventBusOptions> optionsAction)
        {
            Check.NotNull(optionsBuilder, nameof(optionsBuilder));
            Check.NotNull(optionsAction, nameof(optionsAction));

            var options = new RabbitMqEventBusOptions();
            optionsAction?.Invoke(options);
            optionsBuilder.Fx.Services.Configure(optionsAction);

            optionsBuilder.Fx.AddRabbitMq(x =>
            {
                x.Connections.Default.HostName = options.Connections.Default.HostName;
                x.Connections.Default.UserName = options.Connections.Default.UserName;
                x.Connections.Default.Password = options.Connections.Default.Password;
            });

            optionsBuilder.Fx.Services
                .AddSingleton<IEventErrorHandler, RabbitMqEventErrorHandler>()
                .AddSingleton<IEventBus, RabbitMqDistributedEventBus>();

            return optionsBuilder;
        }
    }
}
