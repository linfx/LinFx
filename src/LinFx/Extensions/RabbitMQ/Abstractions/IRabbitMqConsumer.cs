using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LinFx.Extensions.RabbitMq
{
    /// <summary>
    /// 消费者
    /// </summary>
    public interface IRabbitMqConsumer
    {
        Task BindAsync(string routingKey);

        Task UnbindAsync(string routingKey);

        void OnMessageReceived(Func<IModel, BasicDeliverEventArgs, Task> callback);
    }
}