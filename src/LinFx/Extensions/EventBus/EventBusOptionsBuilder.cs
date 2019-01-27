using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.EventBus
{
    public class EventBusOptionsBuilder
    {
        public virtual EventBusOptions Options { get; }
        public ILinFxBuilder Fx { get; }

        public EventBusOptionsBuilder(EventBusOptions options)
        {
            Options = options;
        }

        public EventBusOptionsBuilder(ILinFxBuilder fx, EventBusOptions options)
        {
            Fx = fx;
            Options = options;
        }

        public virtual EventBusOptionsBuilder ReplaceService<TService, TImplementation>()
            where TImplementation : TService
        {
            return this;
        }

        public virtual EventBusOptionsBuilder AddService(IServiceCollection services)
        {
            return this;
        }
    }

    public static class EventBusOptionsExtensions
    {
        public static void AddService(this EventBusOptions options, IServiceCollection services)
        {
        }
    }
}
