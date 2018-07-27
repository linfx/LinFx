using LinFx.Extensions.EventBus.Handlers;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus
{
    /// <summary>
    /// Defines interface of the event bus.
    /// </summary>
    public interface IEventBus
    {
        #region Register

        /// <summary>
        /// Registers to an event.
        /// Given action is called for all event occurrences.
        /// </summary>
        /// <param name="action">Action to handle events</param>
        /// <typeparam name="TEvent">Event type</typeparam>
        void Register<TEvent>(Action<TEvent> action) where TEvent : IEvent;

        /// <summary>
        /// Registers to an event. 
        /// Same (given) instance of the handler is used for all event occurrences.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="handler">Object to handle the event</param>
        void Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;

        /// <summary>
        /// Registers to an event.
        /// A new instance of <see cref="THandler"/> object is created for every event occurrence.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <typeparam name="THandler">Type of the event handler</typeparam>
        void Register<TEvent, THandler>() where TEvent : IEvent where THandler : IEventHandler<TEvent>, new();

        /// <summary>
        /// Registers to an event.
        /// Same (given) instance of the handler is used for all event occurrences.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Object to handle the event</param>
        void Register(Type eventType, IEventHandler handler);

        #endregion

        #region Unregister

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="action"></param>
        void Unregister<TEvent>(Action<TEvent> action) where TEvent : IEvent;

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="handler">Handler object that is registered before</param>
        void Unregister<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Handler object that is registered before</param>
        void Unregister(Type eventType, IEventHandler handler);

        /// <summary>
        /// Unregisters all event handlers of given event type.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        void UnregisterAll<TEvent>() where TEvent : IEvent;

        /// <summary>
        /// Unregisters all event handlers of given event type.
        /// </summary>
        /// <param name="eventType">Event type</param>
        void UnregisterAll(Type eventType);

        #endregion

        #region Trigger

        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="eventData">Related data for the event</param>
        void Trigger<TEvent>(TEvent eventData) where TEvent : IEvent;

        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="eventSource">The object which triggers the event</param>
        /// <param name="eventData">Related data for the event</param>
        void Trigger<TEvent>(object eventSource, TEvent eventData) where TEvent : IEvent;

        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="eventData">Related data for the event</param>
        void Trigger(Type eventType, IEvent eventData);

        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="eventSource">The object which triggers the event</param>
        /// <param name="eventData">Related data for the event</param>
        void Trigger(Type eventType, object eventSource, IEvent eventData);

        /// <summary>
        /// Triggers an event asynchronously.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="eventData">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task TriggerAsync<TEvent>(TEvent eventData) where TEvent : IEvent;

        /// <summary>
        /// Triggers an event asynchronously.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="eventSource">The object which triggers the event</param>
        /// <param name="eventData">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task TriggerAsync<TEvent>(object eventSource, TEvent eventData) where TEvent : IEvent;

        /// <summary>
        /// Triggers an event asynchronously.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="eventData">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task TriggerAsync(Type eventType, IEvent eventData);

        /// <summary>
        /// Triggers an event asynchronously.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="eventSource">The object which triggers the event</param>
        /// <param name="eventData">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task TriggerAsync(Type eventType, object eventSource, IEvent eventData);


        #endregion
    }
}
