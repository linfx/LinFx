namespace LinFx.Extensions.EventBus
{
    public class EventBusOptions
    {
        public string BrokerName { get; set; }

        public string QueueName { get; set; }

        public int RetryCount { get; set; } = 5;

        public bool Durable { get; set; }

        public bool AutoDelete { get; set; }
    }
}
