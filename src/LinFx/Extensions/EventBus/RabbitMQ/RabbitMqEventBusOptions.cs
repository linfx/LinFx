using LinFx.Extensions.RabbitMq;

namespace LinFx.Extensions.EventBus.RabbitMQ
{
    public class RabbitMqEventBusOptions : RabbitMqOptions
    {
        public string Exchange { get; set; }

        public string QueueName { get; set; }
    }
}
