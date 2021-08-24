using LinFx.Collections;

namespace LinFx.Extensions.EventBus.Distributed
{
    public class DistributedEventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }

        public DistributedEventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
        }
    }
}
