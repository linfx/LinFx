namespace LinFx.Extensions.EventBus
{
    public class EventBusOptions
    {
        public string SubscriptionClientName { get; set; }

        public int EventBusRetryCount { get; set; } = 5;
    }
}
