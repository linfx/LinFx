using LinFx.Extensions.RabbitMq;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.RabbitMQ
{
    public class RabbitMqDistributedEventBus : EventBus
    {
        protected RabbitMqEventBusOptions RabbitMqOptions { get; }
        protected IConnectionPool ConnectionPool { get; }
        protected IConsumerFactory ConsumerFactory { get; }
        protected IRabbitMqConsumer Consumer { get; }
        protected IRabbitMqSerializer Serializer { get; }

        public RabbitMqDistributedEventBus(
            IConnectionPool connectionPool,
            IConsumerFactory consumerFactory,
            IRabbitMqSerializer serializer,
            IEventBusSubscriptionsManager subscriptionsManager,
            IServiceProvider serviceProvider,
            IOptions<EventBusOptions> eventBusOptions,
            IOptions<RabbitMqEventBusOptions> rabbitMOptions)
            : base(subscriptionsManager, serviceProvider, eventBusOptions)
        {
            RabbitMqOptions = rabbitMOptions.Value;
            ConnectionPool = connectionPool;
            ConsumerFactory = consumerFactory;
            Serializer = serializer;
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;

            Consumer = ConsumerFactory.Create(
                new ExchangeDeclareConfiguration(
                    RabbitMqOptions.Exchange,
                        type: "direct",
                        durable: true),
                new QueueDeclareConfiguration(
                        RabbitMqOptions.QueueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false),
                RabbitMqOptions.ConnectionName
            );
            Consumer.OnMessageReceived(ProcessEventAsync);
        }

        public override Task PublishAsync(IntegrationEvent evt, string routingKey)
        {
            if (routingKey == default)
            {
                routingKey = evt.GetType().Name;
            }
            var body = Serializer.Serialize(evt);

            using (var channel = ConnectionPool.Get(RabbitMqOptions.ConnectionName).CreateModel())
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = RabbitMqConsts.DeliveryModes.Persistent;

                channel.BasicPublish(
                    exchange: RabbitMqOptions.Exchange,
                    routingKey: routingKey,
                    mandatory: true,
                    basicProperties: properties,
                    body: body
                );
            }

            return Task.CompletedTask;
        }

        public override void Subscribe<TEvent, THandler>()
        {
            var eventName = _subsManager.GetEventKey<TEvent>();
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                _subsManager.AddSubscription<TEvent, THandler>();
                Consumer.BindAsync(eventName);
            }
        }

        private async Task ProcessEventAsync(IModel channel, BasicDeliverEventArgs ea)
        {
            var eventName = ea.RoutingKey;
            var eventData = Encoding.UTF8.GetString(ea.Body);
            await TriggerHandlersAsync(eventName, eventData);
        }
    }
}
