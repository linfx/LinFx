using System.Collections.Concurrent;
namespace LinFx.Extensions.Data
using RabbitMQ.Client;

namespace LinFx.Extensions.RabbitMQ
{
    public class DefaultConnectionPool : IConnectionPool
    {
        private bool _isDisposed;

        protected RabbitMqOptions Options { get; }

        protected ConnectionFactoryWrapper ConnectionFactoryWrapper { get; }

        protected ConcurrentDictionary<string, IConnection> _connections { get; }

        public DefaultConnectionPool(ConnectionFactoryWrapper connectionFactoryWrapper)
        {
            ConnectionFactoryWrapper = connectionFactoryWrapper;
            _connections = new ConcurrentDictionary<string, IConnection>();
        }

        public virtual IConnection Get(string connectionName = null)
        {
            connectionName = connectionName
                 ?? Connections.DefaultConnectionName;

            return _connections.GetOrAdd(
                connectionName, () => 
                ConnectionFactoryWrapper.CreateConnection(connectionName)
            );
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            foreach (var connection in _connections.Values)
            {
                try
                {
                    connection.Dispose();
                }
                catch
                {

                }
            }

            _connections.Clear();
        }
    }
}