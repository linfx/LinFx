//using LinFx.Security.Authorization.Permissions;
//using Microsoft.Extensions.Caching.Distributed;
//using System.Threading.Tasks;

//namespace LinFx.Extensions.PermissionManagement
//{
//    public class PermissionStore : IPermissionStore
//    {
//        protected IPermissionGrantRepository PermissionGrantRepository { get; }

//        protected IDistributedCache Cache { get; }

//        public PermissionStore(
//            IPermissionGrantRepository permissionGrantRepository,
//            IDistributedCache cache)
//        {
//            PermissionGrantRepository = permissionGrantRepository;
//            Cache = cache;
//        }

//        public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
//        {
//            return (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted;
//        }

//        protected virtual async Task<PermissionGrantCacheItem> GetCacheItemAsync(string name, string providerName, string providerKey)
//        {
//            var cacheKey = CalculateCacheKey(name, providerName, providerKey);
//            var cacheItem = await Cache.GetAsync(cacheKey);

//            if (cacheItem != null)
//            {
//                return cacheItem;
//            }

//            cacheItem = new PermissionGrantCacheItem(
//                name,
//                await PermissionGrantRepository.FindAsync(name, providerName, providerKey) != null
//            );

//            await Cache.SetAsync(
//                cacheKey,
//                cacheItem
//            );

//            return cacheItem;
//        }

//        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
//        {
//            return PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);
//        }
//    }
//}
