using System.Collections.Concurrent;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace LinFx.Extensions.RabbitMq
{
    /// <summary>
    /// 默认连接池
    /// </summary>
    public class DefaultConnectionPool : IConnectionPool
    {
        private bool _isDisposed;

        protected RabbitMqOptions Options { get; }

        protected ConnectionFactoryWrapper ConnectionFactoryWrapper { get; }

        protected ConcurrentDictionary<string, IConnection> Connections { get; private set; }

        public DefaultConnectionPool(ConnectionFactoryWrapper connectionFactoryWrapper)
        {
            ConnectionFactoryWrapper = connectionFactoryWrapper;
            Connections = new ConcurrentDictionary<string, IConnection>();
        }

        public virtual IConnection Get(string connectionName = null)
        {
            connectionName ??= RabbitMq.Connections.DefaultConnectionName;

            return Connections.GetOrAdd(connectionName, () => ConnectionFactoryWrapper.CreateConnection(connectionName));
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            foreach (var connection in Connections.Values)
            {
                try
                {
                    connection.Dispose();
                }
                catch
                { }
            }

            Connections.Clear();
        }
    }
}