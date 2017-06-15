namespace LinFx.EventBus.Entities
{
    public class DomainEventEntry
    {
        public object SourceEntity { get; }

        public IEventData EventData { get; }
    }
}
