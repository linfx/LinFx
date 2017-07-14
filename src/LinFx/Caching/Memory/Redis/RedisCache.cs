using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace LinFx.Caching.Memory.Redis
{
	public class RedisCache : CacheBase
	{
		private readonly IDatabase _database;
		private readonly IRedisCacheSerializer _serializer;

		public RedisCache(
			IRedisCacheDatabaseProvider redisCacheDatabaseProvider,
            IRedisCacheSerializer redisCacheSerializer)
        {
            _database = redisCacheDatabaseProvider.GetDatabase();
            _serializer = redisCacheSerializer;
        }

		public override Task<object> GetAsync(string key)
		{
			throw new NotImplementedException();
		}

		public override Task SetAsync(string key, object value, TimeSpan? expireTime = default(TimeSpan?))
		{
			throw new NotImplementedException();
		}
	}
}
