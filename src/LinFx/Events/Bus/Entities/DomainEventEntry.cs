namespace LinFx.Events.Bus.Entities
{
    public class DomainEventEntry
    {
        public object SourceEntity { get; }

        public IEventData EventData { get; }
    }
}
