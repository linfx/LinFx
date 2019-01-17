using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.EventBus
{
    public class EventBusBuilder : IEventBusBuilder
    {
        public ILinFxBuilder Fx { get; }

        public EventBusBuilder(ILinFxBuilder fx)
        {
            Fx = fx;
        }
    }
}
