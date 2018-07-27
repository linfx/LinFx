namespace LinFx.Extensions.EventBus.Handlers
{
    /// <summary>
    /// Undirect base interface for all event handlers.
    /// Implement <see cref="IEventHandler{TEvent}"/> instead of this one.
    /// </summary>
    public interface IEventHandler
    {
    }

    /// <summary>
    /// Defines an interface of a class that handles events of type <see cref="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">Event type to handle</typeparam>
    public interface IEventHandler<in TEvent> : IEventHandler
    {
        /// <summary>
        /// Handler handles the event by implementing this method.
        /// </summary>
        /// <param name="eventData">Event data</param>
        void HandleEvent(TEvent eventData);
    }
}
