using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.EventBus
{
    public interface IEventBusBuilder
    {
        /// <summary>
        /// Gets the <see cref="ILinFxBuilder"/> where Linfx services are configured.
        /// </summary>
        ILinFxBuilder Fx { get; }
    }
}
