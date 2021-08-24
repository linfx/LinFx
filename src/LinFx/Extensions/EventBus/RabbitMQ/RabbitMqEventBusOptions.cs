using LinFx.Extensions.RabbitMq;

namespace LinFx.Extensions.EventBus.RabbitMq
{
    public class RabbitMqEventBusOptions : RabbitMqOptions
    {
        public string ConnectionName { get; set; }

        public string ClientName { get; set; }

        public string ExchangeName { get; set; }
    }
}
