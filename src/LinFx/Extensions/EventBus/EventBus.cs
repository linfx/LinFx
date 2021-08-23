using LinFx.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus
{
    /// <summary>
    /// 事件总线
    /// </summary>
    public abstract class EventBus : IEventBus
    {
        protected EventBusOptions EventBusOptions { get; }
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IEventBusSubscriptionsManager _subsManager;

        protected EventBus(
            [NotNull] IEventBusSubscriptionsManager subsManager,
            [NotNull] IServiceProvider serviceProvider,
            [NotNull] IOptions<EventBusOptions> eventBusOptions)
        {
            Check.NotNull(subsManager, nameof(subsManager));
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            EventBusOptions = eventBusOptions.Value;
            _serviceProvider = serviceProvider;
            _subsManager = subsManager;
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        public abstract Task PublishAsync(IEvent evt, string routingKey = default);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        public abstract void Subscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>;

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        public virtual void Unsubscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
        }

        protected virtual void SubsManager_OnEventRemoved(object sender, string e) { }

        protected virtual async Task TriggerHandlersAsync(string eventName, string eventData)
        {
            var exceptions = new List<Exception>();

            await TriggerHandlersAsync(eventName, eventData, exceptions);

            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                    ExceptionDispatchInfo.Capture(exceptions[0]).Throw();

                throw new AggregateException("More than one error has occurred while triggering the event: " + eventName, exceptions);
            }
        }

        protected virtual async Task TriggerHandlersAsync(string eventName, string eventData, List<Exception> exceptions)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);

                using var scope = _serviceProvider.CreateScope();

                foreach (var subscription in subscriptions)
                {
                    try
                    {
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonUtils.DeserializeObject(eventData, eventType);
                        var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
            }
        }
    }
}