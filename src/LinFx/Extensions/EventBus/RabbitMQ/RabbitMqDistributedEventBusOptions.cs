namespace LinFx.Extensions.EventBus.RabbitMQ
{
    public class RabbitMqDistributedEventBusOptions
    {
        public string ConnectionName { get; set; }

        public string ClientName { get; set; }

        public string ExchangeName { get; set; }
    }
}
