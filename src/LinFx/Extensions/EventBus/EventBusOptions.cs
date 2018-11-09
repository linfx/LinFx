namespace LinFx.Extensions.EventBus
{
    public class EventBusOptions
    {
        public string ProcessExchangeName { get; set; }

        public string ProcessQueueName { get; set; }

        public int EventBusRetryCount { get; set; } = 5;
    }
}
