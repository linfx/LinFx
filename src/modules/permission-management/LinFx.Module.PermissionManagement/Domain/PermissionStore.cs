using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.PermissionManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace LinFx.Extensions.PermissionManagement
{
    public class PermissionStore : IPermissionStore
    {
        private readonly PermissionManagementDbContext _context;

        protected IDistributedCache Cache { get; }

        public PermissionStore(PermissionManagementDbContext context, IDistributedCache cache)
        {
            _context = context;
            Cache = cache;
        }

        public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted;
        }

        protected virtual async Task<PermissionGrantCacheItem> GetCacheItemAsync(string name, string providerName, string providerKey)
        {
            var cacheKey = CalculateCacheKey(name, providerName, providerKey);
            var cacheItem = await Cache.GetAsync<PermissionGrantCacheItem>(cacheKey);

            if (cacheItem != null)
            {
                return cacheItem;
            }

            cacheItem = new PermissionGrantCacheItem(name, await _context.PermissionGrants.AnyAsync(
                p => p.Name == name &&
                p.ProviderName == providerName &&
                p.ProviderKey == providerKey));

            await Cache.SetAsync(cacheKey, cacheItem);

            return cacheItem;
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
