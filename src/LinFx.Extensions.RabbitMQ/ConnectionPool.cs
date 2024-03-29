﻿using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Collections.Concurrent;

namespace LinFx.Extensions.RabbitMQ;

public class ConnectionPool : IConnectionPool
{
    private bool _isDisposed;

    protected RabbitMqOptions Options { get; }

    protected ConcurrentDictionary<string, Lazy<IConnection>> Connections { get; private set; }

    public ConnectionPool(IOptions<RabbitMqOptions> options)
    {
        Options = options.Value;
        Connections = new ConcurrentDictionary<string, Lazy<IConnection>>();
    }

    public virtual IConnection Get(string? connectionName = default)
    {
        connectionName ??= RabbitMqConnections.DefaultConnectionName;

        try
        {
            var lazyConnection = Connections.GetOrAdd(connectionName, () => new Lazy<IConnection>(() =>
            {
                var connection = Options.Connections.GetOrDefault(connectionName);
                var hostnames = connection.HostName.TrimEnd(';').Split(';');
                return hostnames.Length == 1 ? connection.CreateConnection() : connection.CreateConnection(hostnames);
            }));
            return lazyConnection.Value;
        }
        catch (Exception)
        {
            Connections.TryRemove(connectionName, out _);
            throw;
        }
    }

    public virtual void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;

        foreach (var connection in Connections.Values)
        {
            try { connection.Value.Dispose(); } catch { }
        }

        Connections.Clear();
    }
}
