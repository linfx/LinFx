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

        //public static EventBusOptionsBuilder UseRabbitMQ(
        //    [NotNull] this EventBusOptionsBuilder optionsBuilder,
        //    [NotNull] Action<RabbitMqEventBusOptionsBuilder> optionsAction)
        //{
        //    Check.NotNull(optionsBuilder, nameof(optionsBuilder));
        //    Check.NotNull(optionsAction, nameof(optionsAction));

        //    var options = new RabbitMqEventBusOptionsBuilder(
        //        optionsBuilder.Options,
        //        new RabbitMqOptions());

        //    optionsAction?.Invoke(options);

        //    return optionsBuilder;
        //}
    }

    public static class EventBusOptionsExtensions
    {
        public static void UseRabbitMQ(
            [NotNull] this EventBusOptions options,
            [NotNull] ILinFxBuilder fx,
            [NotNull] Action<RabbitMqOptions> optionsAction)
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(optionsAction, nameof(optionsAction));
            Check.NotNull(fx, nameof(fx));

            fx.AddRabbitMQ(optionsAction);
            fx.Services.AddSingleton<IEventBus, RabbitMqDistributedEventBus>();
        }

        //[Obsolete]
        //public static EventBusOptionsBuilder UseRabbitMQ(this EventBusOptionsBuilder optionsBuilder, ILinFxBuilder builder, Action<EventBusRabbitMqOptions> optionsAction)
        //{
        //    Check.NotNull(optionsAction, nameof(optionsAction));

        //    var options = new EventBusRabbitMqOptions();
        //    optionsAction?.Invoke(options);

        //    builder.Services.AddSingleton((Func<IServiceProvider, IRabbitMQPersistentConnection>)(sp =>
        //    {
        //        var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
        //        var factory = new ConnectionFactory
        //        {
        //            UserName = options.UserName,
        //            Password = options.Password,
        //            HostName = options.Host,
        //        };
        //        return new DefaultRabbitMQPersistentConnection(factory, logger);
        //    }));

        //    builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
        //    {
        //        var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
        //        var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
        //        var iServiceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
        //        var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
        //        return new EventBusRabbitMQ(logger, rabbitMQPersistentConnection, eventBusSubcriptionsManager, iServiceScopeFactory, optionsBuilder.Options);
        //    });

        //    return optionsBuilder;
        //}
    }
}
