using LinFx.Extensions.RabbitMq;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.EventBus.RabbitMq
{
    public class RabbitMqEventBusOptionsBuilder : EventBusOptionsBuilder
    {
        public RabbitMqOptions RabbitMqOptions { get; }

        public RabbitMqEventBusOptionsBuilder(
            EventBusOptions options,
            RabbitMqOptions rabbitMqOptions)
            : base(options)
        {
            RabbitMqOptions = rabbitMqOptions;
        }

        public override EventBusOptionsBuilder AddService(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, RabbitMqDistributedEventBus>();
            return this;
        }
    }
}
