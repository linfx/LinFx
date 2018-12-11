using LinFx.Extensions.RabbitMQ;

namespace LinFx.Extensions.EventBus.RabbitMQ
{
    public class EventBusRabbitMQOptionsBuilder
    {
        public RabbitMQOptions RabbitMQOptions { get; set; }

        public EventBusOptions EventBusOptions { get; set; }
    }
}
