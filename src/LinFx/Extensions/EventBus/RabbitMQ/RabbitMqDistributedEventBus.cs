using LinFx.Extensions.RabbitMq;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.RabbitMq
{
    public class RabbitMqDistributedEventBus : EventBus, IDisposable
    {
        protected RabbitMqEventBusOptions RabbitMqEventBusOptions { get; }

        protected IChannelPool ChannelPool { get; }

        protected IConnectionPool ConnectionPool { get; }

        protected IChannelAccessor ChannelAccessor { get; private set; }

        protected IRabbitMqMessageConsumerFactory MessageConsumerFactory { get; }

        /// <summary>
        /// 消费者
        /// </summary>
        protected IRabbitMqMessageConsumer Consumer { get; }

        /// <summary>
        /// 序列化
        /// </summary>
        protected IRabbitMqSerializer Serializer { get; }

        public RabbitMqDistributedEventBus(
            IRabbitMqMessageConsumerFactory consumerFactory,
            IRabbitMqSerializer serializer,
            IEventBusSubscriptionsManager subscriptionsManager,
            IServiceProvider serviceProvider,
            IChannelPool channelPool,
            IOptions<EventBusOptions> eventBusOptions,
            IOptions<RabbitMqEventBusOptions> options)
            : base(subscriptionsManager, serviceProvider, eventBusOptions)
        {
            RabbitMqEventBusOptions = options.Value;
            MessageConsumerFactory = consumerFactory;
            Serializer = serializer;
            ChannelPool = channelPool;
            ChannelAccessor = ChannelPool.Acquire();
            Consumer = MessageConsumerFactory.Create(
                new ExchangeDeclareConfiguration(exchangeName: RabbitMqEventBusOptions.Exchange, type: "direct", durable: true),
                new QueueDeclareConfiguration(queueName: RabbitMqEventBusOptions.QueueName, durable: true, exclusive: false, autoDelete: false),
                RabbitMqEventBusOptions.ConnectionName
            );
            Consumer.OnMessageReceived(ProcessEventAsync);
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        public override Task PublishAsync(IEvent evt, string routingKey)
        {
            if (routingKey == default)
                routingKey = evt.GetType().Name;

            var channel = ChannelAccessor.Channel;
            var body = Serializer.Serialize(evt);
            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = RabbitMqConsts.DeliveryModes.Persistent;
            channel.BasicPublish(RabbitMqEventBusOptions.Exchange, routingKey, true, properties, body);

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
            var eventData = Encoding.UTF8.GetString(ea.Body.Span);
            await TriggerHandlersAsync(eventName, eventData);
        }

        public virtual void Dispose()
        {
            ChannelAccessor?.Dispose();
        }
    }
}
