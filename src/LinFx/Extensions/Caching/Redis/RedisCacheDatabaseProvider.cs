using StackExchange.Redis;
using System;

namespace LinFx.Extensions.Caching.Redis
{
	/// <summary>
	/// Used to get <see cref="IDatabase"/> for Redis cache.
	/// </summary>
	public interface IRedisCacheDatabaseProvider
	{
		/// <summary>
		/// Gets the database connection.
		/// </summary>
		IDatabase GetDatabase();
	}

    public class RedisCacheDatabaseProvider : IRedisCacheDatabaseProvider
	{
		private readonly RedisCacheOptions _options;
		private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

		/// <summary>
		/// Initializes a new instance
		/// </summary>
		public RedisCacheDatabaseProvider(RedisCacheOptions options)
		{
			_options = options;
			_connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
		}

		/// <summary>
		/// Gets the database connection.
		/// </summary>
		public IDatabase GetDatabase()
		{
			return _connectionMultiplexer.Value.GetDatabase(_options.DatabaseId);
		}

		private ConnectionMultiplexer CreateConnectionMultiplexer()
		{
            return ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                Password = _options.Password,
                ChannelPrefix = _options.ChannelPrefix,
                EndPoints = { _options.ConnectionString }
            });
		}
	}
}
