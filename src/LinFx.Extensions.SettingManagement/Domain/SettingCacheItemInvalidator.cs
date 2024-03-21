using LinFx.Domain.Entities.Events;
using LinFx.Extensions.Caching;
using LinFx.Extensions.EventBus.Local;
using System.Threading.Tasks;

namespace LinFx.Extensions.SettingManagement
{
    [Service]
    public class SettingCacheItemInvalidator : ILocalEventHandler<EntityChangedEventData<Setting>>
    {
        protected IDistributedCache<SettingCacheItem> Cache { get; }

        public SettingCacheItemInvalidator(IDistributedCache<SettingCacheItem> cache)
        {
            Cache = cache;
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<Setting> eventData)
        {
            var cacheKey = CalculateCacheKey(
                eventData.Entity.Name,
                eventData.Entity.ProviderName,
                eventData.Entity.ProviderKey
            );

            await Cache.RemoveAsync(cacheKey, considerUow: true);
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return SettingCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
