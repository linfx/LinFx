using System;
using RabbitMQ.Client;

namespace LinFx.Extensions.RabbitMq
{
    public interface IConnectionPool : IDisposable
    {
        IConnection Get(string connectionName = null);
    }
}