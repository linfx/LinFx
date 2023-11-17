using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LinFx.Extensions.RabbitMQ;

/// <summary>
/// 消费者
/// </summary>
public interface IRabbitMqMessageConsumer
{
    Task BindAsync(string routingKey);

    Task UnbindAsync(string routingKey);

    void OnMessageReceived(Func<IModel, BasicDeliverEventArgs, Task> callback);
}
