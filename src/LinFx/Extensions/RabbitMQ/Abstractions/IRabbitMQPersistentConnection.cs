using RabbitMQ.Client;
using System;

namespace LinFx.Extensions.RabbitMq
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
