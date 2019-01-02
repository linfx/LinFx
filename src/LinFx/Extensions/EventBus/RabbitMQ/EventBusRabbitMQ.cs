using LinFx.Extensions.EventBus.Abstractions;
using LinFx.Extensions.EventBus.Events;
using LinFx.Extensions.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly EventBusOptions _options;
        private IModel _consumerChannel;

        public EventBusRabbitMQ(
            ILogger<EventBusRabbitMQ> logger,
            IRabbitMQPersistentConnection persistentConnection,
            IEventBusSubscriptionsManager subsManager, 
            IServiceScopeFactory serviceScopeFactory,
            EventBusOptions options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options;
            _serviceScopeFactory = serviceScopeFactory;
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueUnbind(queue: _options.QueueName,
                    exchange: _options.BrokerName,
                    routingKey: eventName);

                if (_subsManager.IsEmpty)
                {
                    _options.QueueName = string.Empty;
                    _consumerChannel?.Close();
                }
            }
        }

        public Task PublishAsync(IntegrationEvent evt)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_options.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex.ToString());
                });

            using (var channel = _persistentConnection.CreateModel())
            {
                var eventName = evt.GetType().Name;

                channel.ExchangeDeclare(exchange: _options.BrokerName,
                                    type: "direct", 
                                    durable: _options.Durable,
                                    autoDelete: _options.AutoDelete);

                var message = JsonConvert.SerializeObject(evt);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    channel.BasicPublish(exchange: _options.BrokerName,
                                     routingKey: eventName,
                                     mandatory: true,
                                     basicProperties: properties,
                                     body: body);
                });

                return Task.CompletedTask;
            }
        }

        //public void SubscribeDynamic<TH>(string eventName)
        //    where TH : IDynamicIntegrationEventHandler
        //{
        //    DoInternalSubscription(eventName);
        //    _subsManager.AddDynamicSubscription<TH>(eventName);
        //}

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                _subsManager.AddSubscription<T, TH>();

                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                using (var channel = _persistentConnection.CreateModel())
                {
                    if (_consumerChannel == null)
                    {
                        _consumerChannel = CreateConsumerChannel();
                    }

                    channel.QueueBind(queue: _options.QueueName,
                                      exchange: _options.BrokerName,
                                      routingKey: eventName);
                }
            }
        }

        public void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent
        {
            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            _subsManager.Clear();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _options.BrokerName,
                                type: "direct",
                                durable: _options.Durable,
                                autoDelete: _options.AutoDelete);

            channel.QueueDeclare(queue: _options.QueueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body);
                var result = await ProcessEvent(eventName, message);
                if(result)
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
            };


            channel.BasicQos(0, (ushort)_options.PrefetchCount, false);
            channel.BasicConsume(queue: _options.QueueName,
                                 autoAck: false,
                                 consumer: consumer);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            var handler = scope.ServiceProvider.GetService(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                            dynamic eventData = JObject.Parse(message);
                            await handler.Handle(eventData);
                        }
                        else
                        {
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
