using LinFx.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.Test.Caching
{
    public class RedisCachTest
    {
        IDistributedCache _cache;

		public RedisCachTest()
        {
            //string redisConnectionString = "localhost:6379, password=Wayto2017!";
            //string redisConnectionString = "localhost:6379";

            //	var provider = new RedisCacheDatabaseProvider(new RedisCacheOptions
            //	{
            //		ConnectionString = redisConnectionString
            //	});
            //	_cache = new RedisCache(provider, new DefaultRedisCacheSerializer());

            var services = new ServiceCollection();
            //services.AddSingleton<ICache>(p => new RedisCache(new RedisCacheOptions
            //{
            //    Configuration = "1",
            //    InstanceName = "1",
            //}));
            services.AddDistributedRedisCache(options =>
            {
            });
            var container = services.BuildServiceProvider();
            var _cache = container.GetService<IDistributedCache>();
        }


		[Fact]
		public void GetIncrementTest()
		{
			const string KEY_TASK_ID = "LinFx:UserID";
			var id = _cache.StringIncrementAsync(KEY_TASK_ID);
		}

		[Fact]
		public async Task SetTestAsync()
		{
			//string key = "UnitTest.Test1";
			//await _cache.SetAsync(key, 3);
   //         var r = await _cache.GetAsync<int>(key);
		}

		//[Fact]
		//public void GetQueueTest()
		//{
		//	string key = "List.Test";

		//	var t1 = Task.Factory.StartNew(async () =>
		//	{
		//		for(int i = 0; i < 1000; i++)
		//		{
		//			await _cache.ListLeftPushAsync(key, i);
		//		}
		//	});

		//	var t2 = Task.Factory.StartNew(async () =>
		//	{
		//		while(true)
		//		{
		//			var val = await _cache.ListRightPopAsync<int>(key);
		//			Debug.WriteLine(val);
		//			await Task.Delay(1000);
		//		}
		//	});


		//	Task.WaitAll(t1, t2);
		//}
	}
}
