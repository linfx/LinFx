using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace LinFx.Caching.Memory.Redis
{
	public class RedisCache : ICache
	{
		private readonly IDatabase _db;
		private readonly IRedisCacheSerializer _serializer;

		public RedisCache(
			IRedisCacheDatabaseProvider redisCacheDatabaseProvider,
            IRedisCacheSerializer redisCacheSerializer)
        {
            _db = redisCacheDatabaseProvider.GetDatabase();
            _serializer = redisCacheSerializer;
        }

		public async Task<T> GetAsync<T>(string key)
		{
			var val = await _db.StringGetAsync(key);
			return await _serializer.DeserializeAsync<T>(val);
		}

		public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
		{
			var val = await _serializer.SerializeAsync(value);
			await _db.StringSetAsync(key, val, expiry);
		}

		public long GetIncrement(string key)
		{
			return _db.StringIncrement(key);
		}

		public async Task ListLeftPushAsync<T>(string key, T value)
		{
			var val = await _serializer.SerializeAsync(value);
			await _db.ListLeftPushAsync(key, val);
		}

		public async Task<T> ListRightPopAsync<T>(string key)
		{
			string value = await _db.ListRightPopAsync(key);
			return await _serializer.DeserializeAsync<T>(value);
		}
	}
}
