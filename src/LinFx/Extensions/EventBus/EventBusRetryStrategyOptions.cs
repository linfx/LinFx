namespace LinFx.Extensions.EventBus
{
    public class EventBusRetryStrategyOptions
    {
        public int IntervalMillisecond { get; set; } = 3000;

        public int MaxRetryAttempts { get; set; } = 3;
    }
}
