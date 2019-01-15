using LinFx.Extensions.EventBus.Abstractions;
using LinFx.Extensions.EventBus.Events;
using LinFx.Extensions.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.RabbitMQ
{
    public class RabbitMqDistributedEventBus : IEventBus
    {
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        protected RabbitMqDistributedEventBusOptions RabbitMqDistributedEventBusOptions { get; }
        protected DistributedEventBusOptions DistributedEventBusOptions { get; }
        protected IConnectionPool ConnectionPool { get; }
        protected IRabbitMqSerializer Serializer { get; }

        //protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
        protected ConcurrentDictionary<string, Type> EventTypes { get; }
        protected IRabbitMqMessageConsumerFactory MessageConsumerFactory { get; }
        protected IRabbitMqMessageConsumer Consumer { get; }

        public RabbitMqDistributedEventBus(
            IOptions<RabbitMqDistributedEventBusOptions> rabbitMqDistributedEventBusOptions,
            IOptions<DistributedEventBusOptions> distributedEventBusOptions, 
            IConnectionPool connectionPool, 
            ConcurrentDictionary<string, Type> eventTypes, 
            IRabbitMqMessageConsumerFactory messageConsumerFactory,
            IRabbitMqSerializer serializer)
        {
            RabbitMqDistributedEventBusOptions = rabbitMqDistributedEventBusOptions.Value;
            DistributedEventBusOptions = distributedEventBusOptions.Value;
            ConnectionPool = connectionPool;
            EventTypes = eventTypes;
            MessageConsumerFactory = messageConsumerFactory;
            Serializer = serializer;

            Consumer.OnMessageReceived(ProcessEventAsync);
        }

        public Task PublishAsync(IntegrationEvent evt)
        {
            var eventName = evt.GetType().Name;
            var body = Serializer.Serialize(evt);

            using (var channel = ConnectionPool.Get(RabbitMqDistributedEventBusOptions.ConnectionName).CreateModel())
            {
                channel.ExchangeDeclare(
                    RabbitMqDistributedEventBusOptions.ExchangeName,
                    "direct",
                    durable: true
                );

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = RabbitMqConsts.DeliveryModes.Persistent;

                channel.BasicPublish(
                   exchange: RabbitMqDistributedEventBusOptions.ExchangeName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body
                );
            }

            return Task.CompletedTask;
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                _subsManager.AddSubscription<T, TH>();

                Consumer.BindAsync(eventName);
            }
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            throw new NotImplementedException();
        }

        private async Task ProcessEventAsync(IModel channel, BasicDeliverEventArgs ea)
        {
            var eventName = ea.RoutingKey;
            var eventType = EventTypes.GetOrDefault(eventName);
            if (eventType == null)
            {
                return;
            }

            var eventData = Serializer.Deserialize(ea.Body, eventType);
            await TriggerHandlersAsync(eventName, eventData);
        }

        public virtual async Task TriggerHandlersAsync(string eventName, object eventData)
        {
            var exceptions = new List<Exception>();

            await TriggerHandlersAsync(eventName, eventData, exceptions);

            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                {
                    exceptions[0].ReThrow();
                }

                throw new AggregateException("More than one error has occurred while triggering the event: " + eventName, exceptions);
            }
        }

        protected virtual async Task TriggerHandlersAsync(string eventName, object eventData, List<Exception> exceptions)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        try
                        {
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { eventData });
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
}
