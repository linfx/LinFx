using StackExchange.Redis;
using System;

namespace LinFx.Caching.Redis
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
		/// Initializes a new instance of the <see cref="AbpRedisCacheDatabaseProvider"/> class.
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
			return ConnectionMultiplexer.Connect(_options.ConnectionString);
		}
	}
}
