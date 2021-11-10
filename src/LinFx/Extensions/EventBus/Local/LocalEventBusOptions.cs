using LinFx.Collections;

namespace LinFx.Extensions.EventBus.Local
{
    public class LocalEventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }

        public LocalEventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
        }
    }
}