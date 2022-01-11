using LinFx.Extensions.EventBus.Distributed;
using LinFx.Extensions.EventBus.Local;

namespace LinFx.Extensions.EventBus;

/// <summary>
/// Undirect base interface for all event handlers.
/// Implement <see cref="ILocalEventHandler{TEvent}"/> or <see cref="IDistributedEventHandler{TEvent}"/> instead of this one.
/// </summary>
public interface IEventHandler
{
}
