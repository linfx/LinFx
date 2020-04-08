using LinFx.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace LinFx.Extensions.RabbitMq
{
    /// <summary>
    /// 消费者
    /// </summary>
    public class RabbitMqConsumer : IRabbitMqConsumer, IDisposable
    {
        public ILogger<RabbitMqConsumer> Logger { get; set; }

        /// <summary>
        /// 连接池
        /// </summary>
        protected IConnectionPool ConnectionPool { get; }

        /// <summary>
        /// Common AMQP model
        /// </summary>
        protected IModel Channel { get; private set; }
        protected Timer Timer { get; }

        protected ExchangeDeclareConfiguration Exchange { get; private set; }
        protected QueueDeclareConfiguration Queue { get; private set; }

        protected string ConnectionName { get; private set; }

        protected ConcurrentBag<Func<IModel, BasicDeliverEventArgs, Task>> Callbacks { get; }
        protected ConcurrentQueue<QueueBindCommand> QueueBindCommands { get; }

        protected object ChannelSendSyncLock { get; } = new object();

        public RabbitMqConsumer(IConnectionPool connectionPool)
        {
            Logger = NullLogger<RabbitMqConsumer>.Instance;
            ConnectionPool = connectionPool;

            QueueBindCommands = new ConcurrentQueue<QueueBindCommand>();
            Callbacks = new ConcurrentBag<Func<IModel, BasicDeliverEventArgs, Task>>();

            Timer = new Timer
            {
                Period = 5000, //5 sec.
            };
            Timer.Elapsed += Timer_Elapsed;
        }

        public void Initialize(
            [NotNull] ExchangeDeclareConfiguration exchange,
            [NotNull] QueueDeclareConfiguration queue,
            string connectionName = null)
        {
            Exchange = Check.NotNull(exchange, nameof(exchange));
            Queue = Check.NotNull(queue, nameof(queue));
            ConnectionName = connectionName;
            Timer.StartAsync().Wait();
        }

        public virtual async Task BindAsync(string routingKey)
        {
            QueueBindCommands.Enqueue(new QueueBindCommand(QueueBindType.Bind, routingKey));
            await TrySendQueueBindCommandsAsync();
        }

        public virtual async Task UnbindAsync(string routingKey)
        {
            QueueBindCommands.Enqueue(new QueueBindCommand(QueueBindType.Unbind, routingKey));
            await TrySendQueueBindCommandsAsync();
        }

        protected virtual Task TrySendQueueBindCommandsAsync()
        {
            try
            {
                while (!QueueBindCommands.IsEmpty)
                {
                    if (Channel == null || Channel.IsClosed)
                    {
                        return Task.CompletedTask;
                    }

                    lock (ChannelSendSyncLock)
                    {
                        QueueBindCommands.TryPeek(out var command);

                        switch (command.Type)
                        {
                            case QueueBindType.Bind:
                                Channel.QueueBind(
                                    queue: Queue.QueueName,
                                    exchange: Exchange.ExchangeName,
                                    routingKey: command.RoutingKey
                                );
                                break;
                            case QueueBindType.Unbind:
                                Channel.QueueUnbind(
                                    queue: Queue.QueueName,
                                    exchange: Exchange.ExchangeName,
                                    routingKey: command.RoutingKey
                                );
                                break;
                            default:
                                throw new LinFxException($"Unknown {nameof(QueueBindType)}: {command.Type}");
                        }

                        QueueBindCommands.TryDequeue(out command);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }

            return Task.CompletedTask;
        }

        public virtual void OnMessageReceived(Func<IModel, BasicDeliverEventArgs, Task> callback)
        {
            Callbacks.Add(callback);
        }

        protected virtual void Timer_Elapsed(object sender, EventArgs e)
        {
            if (Channel == null || Channel.IsOpen == false)
            {
                TryCreateChannel();
                //AsyncHelper.RunSync(TrySendQueueBindCommandsAsync);
                //AsyncHelper.Wait.Run(TrySendQueueBindCommandsAsync());
                TrySendQueueBindCommandsAsync().Wait();
            }
        }

        protected virtual void TryCreateChannel()
        {
            DisposeChannel();

            try
            {
                var channel = ConnectionPool.Get(ConnectionName).CreateModel();

                channel.ExchangeDeclare(exchange: Exchange.ExchangeName,
                                        type: Exchange.Type,
                                        durable: Exchange.Durable,
                                        autoDelete: Exchange.AutoDelete,
                                        arguments: Exchange.Arguments
                );

                channel.QueueDeclare(queue: Queue.QueueName,
                                     durable: Queue.Durable,
                                     exclusive: Queue.Exclusive,
                                     autoDelete: Queue.AutoDelete,
                                     arguments: Queue.Arguments
                );

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    await HandleIncomingMessage(channel, ea);
                };

                channel.BasicConsume(queue: Queue.QueueName,
                                     autoAck: false,
                                     consumer: consumer
                );

                Channel = channel;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }

        protected virtual async Task HandleIncomingMessage(IModel channel, BasicDeliverEventArgs ea)
        {
            try
            {
                foreach (var callback in Callbacks)
                {
                    await callback(channel, ea);
                }
                channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }

        protected virtual void DisposeChannel()
        {
            if (Channel == null)
                return;

            try
            {
                Channel.Dispose();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }

        public virtual void Dispose()
        {
            Timer.StartAsync().Wait();
            DisposeChannel();
        }

        protected class QueueBindCommand
        {
            public QueueBindType Type { get; }

            public string RoutingKey { get; }

            public QueueBindCommand(QueueBindType type, string routingKey)
            {
                Type = type;
                RoutingKey = routingKey;
            }
        }

        protected enum QueueBindType
        {
            Bind,
            Unbind
        }
    }
}
