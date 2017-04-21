using System;
using System.Threading.Tasks;
using LinFx.Events.Bus.Factories;
using LinFx.Events.Bus.Handlers;

namespace LinFx.Events.Bus
{
    public class NullEventBus : IEventBus
    {
        public IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public IDisposable Register<TEventData, THandler>()
            where TEventData : IEventData
            where THandler : IEventHandler<TEventData>, new()
        {
            throw new NotImplementedException();
        }

        public IDisposable Register(Type eventType, IEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public IDisposable Register<TEventData>(IEventHandlerFactory handlerFactory) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory)
        {
            throw new NotImplementedException();
        }

        public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void Trigger(Type eventType, IEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void Trigger(Type eventType, object eventSource, IEventData eventData)
        {
            throw new NotImplementedException();
        }

        public Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public Task TriggerAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public Task TriggerAsync(Type eventType, IEventData eventData)
        {
            throw new NotImplementedException();
        }

        public Task TriggerAsync(Type eventType, object eventSource, IEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void Unregister(Type eventType, IEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void Unregister(Type eventType, IEventHandlerFactory factory)
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll<TEventData>() where TEventData : IEventData
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll(Type eventType)
        {
            throw new NotImplementedException();
        }
    }
}
