namespace LinFx.Extensions.EventBus
{
    public class EventBusOptionsBuilder
    {
        public EventBusOptionsBuilder(EventBusOptions options)
        {
            Options = options;
        }

        public virtual EventBusOptions Options { get; }
    }
}
