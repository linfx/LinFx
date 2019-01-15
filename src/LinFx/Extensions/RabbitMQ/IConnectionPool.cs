using System;
using RabbitMQ.Client;

namespace LinFx.Extensions.RabbitMQ
{
    public interface IConnectionPool : IDisposable
    {
        IConnection Get(string connectionName = null);
    }
}