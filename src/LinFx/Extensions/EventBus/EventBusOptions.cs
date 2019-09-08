namespace LinFx.Extensions.EventBus
{
    public class EventBusOptions
    {
        public int RetryCount { get; set; } = 3;

        public int FailCount { get; set; } = 3;

        public bool Durable { get; set; }

        public bool AutoDelete { get; set; }

        public int PrefetchCount { get; set; } = 1;
    }
}
