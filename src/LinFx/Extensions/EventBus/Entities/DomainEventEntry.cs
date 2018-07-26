namespace LinFx.Extensions.EventBus.Entities
{
    public class DomainEventEntry
    {
        public object SourceEntity { get; }

        public IEventData EventData { get; }
    }
}
