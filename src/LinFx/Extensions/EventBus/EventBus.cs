using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using LinFx.Extensions.EventBus.Handlers;
using LinFx.Extensions.EventBus.Handlers.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LinFx.Extensions.EventBus
{
    public class EventBus : IEventBus
    {
        readonly IServiceCollection _services;
        readonly IServiceProvider _provider;
        readonly ILogger _logger;

        public EventBus(IServiceProvider provider)
        {
            _provider = provider;
            //_services = services;
        }

        public void Register<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
            Register(typeof(TEvent), new ActionEventHandler<TEvent>(action));
        }

        public void Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            _services.Add(new ServiceDescriptor(handler.GetType(), handler));
        }

        public void Register<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>, new()
        {
            throw new NotImplementedException();
        }

        public void Register(Type eventType, IEventHandler handler)
        {
            _services.AddTransient(eventType, handler.GetType());
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

        public void Trigger(Type eventType, object eventSource, IEvent ent)
        {
            var exceptions = new List<Exception>();

            ent.EventSource = eventSource;
            var handlers = _provider.GetServices<IEventHandler<IEvent>>();

            foreach (var item in handlers)
            {
                item.HandleEvent(ent);
            }

            var handler = _provider.GetService(eventType);
            handlers.AsParallel().ForAll(eventHandler => eventHandler.HandleEvent(ent));

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
                    _logger.LogError(ex, ex.Message);
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

        public void UnregisterAll<TEvent>() where TEvent : IEvent
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll(Type eventType)
        {
            throw new NotImplementedException();
        }
    }
}
