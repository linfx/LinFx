using LinFx.Extensions.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using static LinFx.Extensions.EventBus.InMemoryEventBusSubscriptionsManager;

namespace LinFx.Extensions.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }

        event EventHandler<string> OnEventRemoved;
        
        void AddSubscription<TEvent, THandler>()
           where TEvent : IEvent
           where THandler : IEventHandler<TEvent>;

        void RemoveSubscription<TEvent, THandler>()
             where TEvent : IEvent
             where THandler : IEventHandler<TEvent>;
        
        bool HasSubscriptionsForEvent<TEvent>() where TEvent : IEvent;

        bool HasSubscriptionsForEvent(string eventName);

        Type GetEventTypeByName(string eventName);

        void Clear();

        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IEvent;

        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        string GetEventKey<T>();
    }
}
