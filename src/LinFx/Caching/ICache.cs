using System;
using System.Threading.Tasks;

namespace LinFx.Caching
{
    public interface ICache
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        void SetAsync(string key, object data, int cacheTime);
    }

    //public static class CacheExtensions
    //{
    //    public static void SetAsync(string key, object data, int cacheTime)
    //    {
    //        return Get(cacheManager, key, 60, acquire);
    //    }
    //}
}
