using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.EventBus.Local;

namespace LinFx.Extensions.EventBus;

/// <summary>
/// This event handler is an adapter to be able to use an action as <see cref="ILocalEventHandler{TEvent}"/> implementation.
/// </summary>
/// <typeparam name="TEvent">Event type</typeparam>
/// <remarks>
/// Creates a new instance of <see cref="ActionEventHandler{TEvent}"/>.
/// </remarks>
/// <param name="handler">Action to handle the event</param>
[Service]
public class ActionEventHandler<TEvent>(Func<TEvent, Task> handler) : ILocalEventHandler<TEvent>
{
    /// <summary>
    /// Function to handle the event.
    /// </summary>
    public Func<TEvent, Task> Action { get; } = handler;

    /// <summary>
    /// Handles the event.
    /// </summary>
    /// <param name="eventData"></param>
    public async Task HandleEventAsync(TEvent eventData) => await Action(eventData);
}
