using RabbitMQ.Client;
using System;

namespace LinFx.Extensions.RabbitMQ
{
    public interface IPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }

    /// <summary>
    /// Use <see cref="IPersistentConnection"/>
    /// </summary>
    [Obsolete]
    public interface IRabbitMQPersistentConnection : IPersistentConnection
    {
    }
}
