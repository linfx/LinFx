using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LinFx.Extensions.RabbitMQ
{
    public interface IRabbitMqConsumer
    {
        Task BindAsync(string routingKey);

        Task UnbindAsync(string routingKey);

        void OnMessageReceived(Func<IModel, BasicDeliverEventArgs, Task> callback);
    }
}