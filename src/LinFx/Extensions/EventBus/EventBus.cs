using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LinFx.Extensions.EventBus.Handlers;
using LinFx.Extensions.EventBus.Factories.Internals;
using LinFx.Extensions.EventBus.Handlers.Internals;
using Microsoft.Extensions.Logging;
using LinFx.Extensions.EventBus.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.EventBus
{
    public class EventBus : IEventBus
    {
        readonly IServiceCollection _services;
        readonly IServiceProvider _container;

        /// <summary>
        /// Reference to the Logger.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// All registered handler factories.
        /// Key: Type of the event
        /// Value: List of handler factories
        /// </summary>
        private readonly ConcurrentDictionary<Type, List<IEventHandlerFactory>> _handlerFactories;

        /// <summary>
        /// Creates a new <see cref="EventBus"/> instance.
        /// </summary>
        public EventBus(IServiceCollection services)
        {
            _handlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            _services = services;
        }

        public void Register<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
            Register(typeof(TEvent), new ActionEventHandler<TEvent>(action));
        }

        public void Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            //_services.AddTransient<IEventHandler<TEvent>, handler>()
        }

        public void Register<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>, new()
        {
            throw new NotImplementedException();
        }

        public void Register(Type eventType, IEventHandler handler)
        {
            //Register(eventType, new SingleInstanceHandlerFactory(handler));
            _services.AddTransient(eventType, handler.GetType());
        }

        public void Register<TEvent>(IEventHandlerFactory handlerFactory) where TEvent : IEvent
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Register(Type eventType, IEventHandlerFactory handlerFactory)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Add(handlerFactory));
            new FactoryUnregistrar(this, eventType, handlerFactory);
        }

        public void Trigger<TEvent>(TEvent eventData) where TEvent : IEvent
        {
            Trigger((object)null, eventData);
        }

        public void Trigger<TEvent>(object eventSource, TEvent eventData) where TEvent : IEvent
        {
            Trigger(typeof(TEvent), eventSource, eventData);
        }

        public void Trigger(Type eventType, IEvent eventData)
        {
            Trigger(eventType, null, eventData);
        }

        public void Trigger(Type eventType, object eventSource, IEvent eventData)
        {
            var exceptions = new List<Exception>();
            TriggerHandlingException(eventType, eventSource, eventData, exceptions);
            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                {
                    exceptions[0].ReThrow();
                }
                throw new AggregateException("More than one error has occurred while triggering the event: " + eventType, exceptions);
            }
        }

        public Task TriggerAsync<TEvent>(TEvent eventData) where TEvent : IEvent
        {
            throw new NotImplementedException();
        }

        public Task TriggerAsync<TEvent>(object eventSource, TEvent eventData) where TEvent : IEvent
        {
            throw new NotImplementedException();
        }

        public Task TriggerAsync(Type eventType, IEvent eventData)
        {
            throw new NotImplementedException();
        }

        public Task TriggerAsync(Type eventType, object eventSource, IEvent eventData)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    Trigger(eventSource, eventData);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, ex.Message);
                }
            });
        }

        public void Unregister<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
            throw new NotImplementedException();
        }

        public void Unregister<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            throw new NotImplementedException();
        }

        public void Unregister(Type eventType, IEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public void Unregister<TEvent>(IEventHandlerFactory factory) where TEvent : IEvent
        {
            throw new NotImplementedException();
        }

        public void Unregister(Type eventType, IEventHandlerFactory factory)
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll<TEvent>() where TEvent : IEvent
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll(Type eventType)
        {
            throw new NotImplementedException();
        }

        #region Private Method

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return _handlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
        }

        private void TriggerHandlingException(Type eventType, object eventSource, IEvent evenDatat, List<Exception> exceptions)
        {
            evenDatat.EventSource = eventSource;
            var handlers = _container.GetServices<IEventHandler<IEvent>>();
            handlers.AsParallel().ForAll(eventHandler => eventHandler.HandleEvent(evenDatat));
        }

        private IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in _handlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
        }

        private static bool ShouldTriggerEventForHandler(Type eventType, Type handlerType)
        {
            //Should trigger same type
            if (handlerType == eventType)
            {
                return true;
            }

            //Should trigger for inherited types
            //if (handlerType.IsAssignableFrom(eventType))
            //{
            //    return true;
            //}

            return false;
        }

        private class EventTypeWithEventHandlerFactories
        {
            public Type EventType { get; }

            public List<IEventHandlerFactory> EventHandlerFactories { get; }

            public EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
            {
                EventType = eventType;
                EventHandlerFactories = eventHandlerFactories;
            }
        }


        #endregion
    }
}
