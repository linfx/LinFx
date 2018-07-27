using LinFx.Extensions.EventBus;

namespace LinFx.Test.Extensions.EventBus.Events
{
    public class TodoCreatedEvent :Event
    {
        public string Name { get; set; }
    }
}
