using Confluent.Kafka;
using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace LinFx.Extensions.Kafka;

/// <summary>
/// 生产者池
/// </summary>
public class ProducerPool : IProducerPool, ISingletonDependency
{
    protected KafkaOptions Options { get; }

    /// <summary>
    /// 生产者
    /// </summary>
    protected ConcurrentDictionary<string, Lazy<IProducer<string, byte[]>>> Producers { get; } = new ConcurrentDictionary<string, Lazy<IProducer<string, byte[]>>>();

    protected TimeSpan TotalDisposeWaitDuration { get; set; } = TimeSpan.FromSeconds(10);

    protected TimeSpan DefaultTransactionsWaitDuration { get; set; } = TimeSpan.FromSeconds(30);

    public ILogger<ProducerPool> Logger { get; set; } = new NullLogger<ProducerPool>();

    private bool _isDisposed;

    public ProducerPool(IOptions<KafkaOptions> options)
    {
        Options = options.Value;
    }

    /// <summary>
    /// 获取生产者
    /// </summary>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public virtual IProducer<string, byte[]> Get(string connectionName = null)
    {
        connectionName ??= KafkaConnections.DefaultConnectionName;

        return Producers.GetOrAdd(connectionName, connection => new Lazy<IProducer<string, byte[]>>(() =>
        {
            var producerConfig = new ProducerConfig(Options.Connections.GetOrDefault(connection));
            Options.ConfigureProducer?.Invoke(producerConfig);
            return new ProducerBuilder<string, byte[]>(producerConfig).Build();

        })).Value;
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;

        if (!Producers.Any())
        {
            Logger.LogDebug($"Disposed producer pool with no producers in the pool.");
            return;
        }

        var poolDisposeStopwatch = Stopwatch.StartNew();

        Logger.LogInformation($"Disposing producer pool ({Producers.Count} producers).");

        var remainingWaitDuration = TotalDisposeWaitDuration;

        foreach (var producer in Producers.Values)
        {
            var poolItemDisposeStopwatch = Stopwatch.StartNew();

            try
            {
                producer.Value.Dispose();
            }
            catch
            {
            }

            poolItemDisposeStopwatch.Stop();

            remainingWaitDuration = remainingWaitDuration > poolItemDisposeStopwatch.Elapsed
                ? remainingWaitDuration.Subtract(poolItemDisposeStopwatch.Elapsed)
                : TimeSpan.Zero;
        }

        poolDisposeStopwatch.Stop();

        Logger.LogInformation(
            $"Disposed Kafka Producer Pool ({Producers.Count} producers in {poolDisposeStopwatch.Elapsed.TotalMilliseconds:0.00} ms).");

        if (poolDisposeStopwatch.Elapsed.TotalSeconds > 5.0)
        {
            Logger.LogWarning(
                $"Disposing Kafka Producer Pool got time greather than expected: {poolDisposeStopwatch.Elapsed.TotalMilliseconds:0.00} ms.");
        }

        Producers.Clear();
    }
}
