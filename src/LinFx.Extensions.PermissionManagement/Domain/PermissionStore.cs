using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.Caching;
using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LinFx.Extensions.PermissionManagement;

[Service]
public class PermissionStore(
    PermissionManagementDbContext db,
    IDistributedCache<PermissionGrantCacheItem> cache,
    IPermissionDefinitionManager permissionDefinitionManager) : IPermissionStore
{
    public ILogger<PermissionStore> Logger { get; set; } = NullLogger<PermissionStore>.Instance;

    protected PermissionManagementDbContext Db { get; } = db;

    protected IPermissionDefinitionManager PermissionDefinitionManager { get; } = permissionDefinitionManager;

    protected IDistributedCache<PermissionGrantCacheItem> Cache { get; } = cache;

    public virtual async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey) => (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted;

    protected virtual async Task<PermissionGrantCacheItem> GetCacheItemAsync(
        string name,
        string providerName,
        string providerKey)
    {
        var cacheKey = CalculateCacheKey(name, providerName, providerKey);

        Logger.LogDebug($"PermissionStore.GetCacheItemAsync: {cacheKey}");

        var cacheItem = await Cache.GetAsync(cacheKey);

        if (cacheItem != null)
        {
            Logger.LogDebug($"Found in the cache: {cacheKey}");
            return cacheItem;
        }

        Logger.LogDebug($"Not found in the cache: {cacheKey}");

        cacheItem = new PermissionGrantCacheItem(false);
        await SetCacheItemsAsync(providerName, providerKey, name, cacheItem);
        return cacheItem;
    }

    protected virtual async Task SetCacheItemsAsync(
        string providerName,
        string providerKey,
        string currentName,
        PermissionGrantCacheItem currentCacheItem)
    {
        var permissions = PermissionDefinitionManager.GetPermissions();

        Logger.LogDebug($"Getting all granted permissions from the repository for this provider name,key: {providerName},{providerKey}");

        var grantedPermissionsHashSet = new HashSet<string>(Db.PermissionGrants.Where(s => s.ProviderName == providerName && s.ProviderKey == providerKey).Select(p => p.Name));

        Logger.LogDebug($"Setting the cache items. Count: {permissions.Count}");

        var cacheItems = new List<KeyValuePair<string, PermissionGrantCacheItem>>();

        foreach (var permission in permissions)
        {
            var isGranted = grantedPermissionsHashSet.Contains(permission.Name);

            cacheItems.Add(new KeyValuePair<string, PermissionGrantCacheItem>(CalculateCacheKey(permission.Name, providerName, providerKey), new PermissionGrantCacheItem(isGranted)));

            if (permission.Name == currentName)
            {
                currentCacheItem.IsGranted = isGranted;
            }
        }

        await Cache.SetManyAsync(cacheItems);

        Logger.LogDebug($"Finished setting the cache items. Count: {permissions.Count}");
    }

    public virtual async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
    {
        Check.NotNullOrEmpty(names, nameof(names));

        var result = new MultiplePermissionGrantResult();

        if (names.Length == 1)
        {
            var name = names.First();
            result.Result.Add(name,
                await IsGrantedAsync(names.First(), providerName, providerKey)
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Undefined);
            return result;
        }

        var cacheItems = await GetCacheItemsAsync(names, providerName, providerKey);
        foreach (var item in cacheItems)
        {
            result.Result.Add(GetPermissionNameFormCacheKeyOrNull(item.Key),
                item.Value != null && item.Value.IsGranted
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Undefined);
        }

        return result;
    }

    protected virtual async Task<List<KeyValuePair<string, PermissionGrantCacheItem>>> GetCacheItemsAsync(
        string[] names,
        string providerName,
        string providerKey)
    {
        var cacheKeys = names.Select(x => CalculateCacheKey(x, providerName, providerKey)).ToList();

        Logger.LogDebug($"PermissionStore.GetCacheItemAsync: {string.Join(",", cacheKeys)}");

        var cacheItems = (await Cache.GetManyAsync(cacheKeys)).ToList();
        if (cacheItems.All(x => x.Value != null))
        {
            Logger.LogDebug($"Found in the cache: {string.Join(",", cacheKeys)}");
            return cacheItems;
        }

        var notCacheKeys = cacheItems.Where(x => x.Value == null).Select(x => x.Key).ToList();

        Logger.LogDebug($"Not found in the cache: {string.Join(",", notCacheKeys)}");

        var newCacheItems = await SetCacheItemsAsync(providerName, providerKey, notCacheKeys);

        var result = new List<KeyValuePair<string, PermissionGrantCacheItem>>();
        foreach (var key in cacheKeys)
        {
            var item = newCacheItems.FirstOrDefault(x => x.Key == key);
            if (item.Value == null)
            {
                item = cacheItems.FirstOrDefault(x => x.Key == key);
            }

            result.Add(new KeyValuePair<string, PermissionGrantCacheItem>(key, item.Value));
        }

        return result;
    }

    protected virtual async Task<List<KeyValuePair<string, PermissionGrantCacheItem>>> SetCacheItemsAsync(
        string providerName,
        string providerKey,
        List<string> notCacheKeys)
    {
        var permissions = PermissionDefinitionManager.GetPermissions().Where(x => notCacheKeys.Any(k => GetPermissionNameFormCacheKeyOrNull(k) == x.Name)).ToList();

        Logger.LogDebug($"Getting not cache granted permissions from the repository for this provider name,key: {providerName},{providerKey}");

        var names = notCacheKeys.Select(GetPermissionNameFormCacheKeyOrNull).ToArray();
        var collect = Db.PermissionGrants.Where(s => names.Contains(s.Name) && s.ProviderName == providerName && s.ProviderKey == providerKey).Select(p => p.Name);
        var grantedPermissionsHashSet = new HashSet<string>(collect);


        Logger.LogDebug($"Setting the cache items. Count: {permissions.Count}");

        var cacheItems = new List<KeyValuePair<string, PermissionGrantCacheItem>>();

        foreach (var permission in permissions)
        {
            var isGranted = grantedPermissionsHashSet.Contains(permission.Name);

            cacheItems.Add(new KeyValuePair<string, PermissionGrantCacheItem>(
                CalculateCacheKey(permission.Name, providerName, providerKey),
                new PermissionGrantCacheItem(isGranted))
            );
        }

        await Cache.SetManyAsync(cacheItems);

        Logger.LogDebug($"Finished setting the cache items. Count: {permissions.Count}");

        return cacheItems;
    }

    protected virtual string CalculateCacheKey(string name, string providerName, string providerKey) => PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);

    protected virtual string? GetPermissionNameFormCacheKeyOrNull(string key) => PermissionGrantCacheItem.GetPermissionNameFormCacheKeyOrNull(key);
}
