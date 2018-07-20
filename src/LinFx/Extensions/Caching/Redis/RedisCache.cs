using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Caching.Redis
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
            if (val.HasValue)
                return await _serializer.DeserializeAsync<T>(val);
            return default(T);
        }

		public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
		{
			var val = await _serializer.SerializeAsync(value);
			await _db.StringSetAsync(key, val, expiry);
		}

        public Task<bool> IsExistsAsync(string key)
        {
            return _db.KeyExistsAsync(key);
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

        #region Hash

        //public Task<bool> HashSetAsync(string key, string hashField, string value)
        //{
        //    return _db.HashSetAsync(key, hashField, value);
        //}

        public Task HashSetAsync(string key, HashEntry[] hashFields)
        {
            return _db.HashSetAsync(key, hashFields);
        }

        public Task<long> HashIncrementAsync(string key, string hashField)
        {
            return _db.HashIncrementAsync(key, hashField);
        }

        public Task<long> HashDecrementAsync(string key, string hashField)
        {
            return _db.HashDecrementAsync(key, hashField);
        }

        #endregion
    }
}
