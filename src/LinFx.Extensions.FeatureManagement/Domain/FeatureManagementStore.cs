using LinFx.Extensions.Caching;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Features;
using LinFx.Extensions.Uow;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.FeatureManagement;

public class FeatureManagementStore(
    FeatureManagementDbContext db,
    IDistributedCache<FeatureValueCacheItem> cache,
    IFeatureDefinitionManager featureDefinitionManager) : IFeatureManagementStore, ITransientDependency
{
    protected IDistributedCache<FeatureValueCacheItem> Cache { get; } = cache;

    protected IFeatureDefinitionManager FeatureDefinitionManager { get; } = featureDefinitionManager;

    protected FeatureManagementDbContext Db { get; } = db;

    [UnitOfWork]
    public virtual async Task<string?> GetOrNullAsync(string name, string providerName, string providerKey) => (await GetCacheItemAsync(name, providerName, providerKey)).Value;

    [UnitOfWork]
    public virtual async Task SetAsync(string name, string value, string providerName, string providerKey)
    {
        var featureValue = await Db.FeatureValues.FindAsync(name, providerName, providerKey);
        if (featureValue == null)
        {
            featureValue = new FeatureValue(IDUtils.NewIdString(), name, value, providerName, providerKey);
            //await Db.FeatureValues.InsertAsync(featureValue);
            Db.FeatureValues.Add(featureValue);
        }
        else
        {
            featureValue.Value = value;
            //await Db.FeatureValues.UpdateAsync(featureValue);
        }
        await Cache.SetAsync(CalculateCacheKey(name, providerName, providerKey), new FeatureValueCacheItem(featureValue.Value), considerUow: true);
    }

    [UnitOfWork]
    public virtual Task DeleteAsync(string name, string providerName, string providerKey)
    {
        //var featureValues = await Db.FindAllAsync(name, providerName, providerKey);
        //foreach (var featureValue in featureValues)
        //{
        //    await Db.DeleteAsync(featureValue);
        //    await Cache.RemoveAsync(CalculateCacheKey(name, providerName, providerKey), considerUow: true);
        //}
        throw new NotImplementedException();
    }

    protected virtual async Task<FeatureValueCacheItem> GetCacheItemAsync(string name, string providerName, string providerKey)
    {
        var cacheKey = CalculateCacheKey(name, providerName, providerKey);
        var cacheItem = await Cache.GetAsync(cacheKey, considerUow: true);
        if (cacheItem != null)
            return cacheItem;

        cacheItem = new FeatureValueCacheItem();
        await SetCacheItemsAsync(providerName, providerKey, name, cacheItem);
        return cacheItem;
    }

    private async Task SetCacheItemsAsync(
        string providerName,
        string providerKey,
        string currentName,
        FeatureValueCacheItem currentCacheItem)
    {
        var featureDefinitions = await FeatureDefinitionManager.GetAllAsync();
        var featuresDictionary = await Db.FeatureValues.Where(p => p.ProviderName == providerName && p.ProviderKey == providerKey).ToDictionaryAsync(s => s.Name, s => s.Value);

        var cacheItems = new List<KeyValuePair<string, FeatureValueCacheItem>>();

        foreach (var featureDefinition in featureDefinitions)
        {
            var featureValue = featuresDictionary.GetOrDefault(featureDefinition.Name);

            cacheItems.Add(
                new KeyValuePair<string, FeatureValueCacheItem>(
                    CalculateCacheKey(featureDefinition.Name, providerName, providerKey),
                    new FeatureValueCacheItem(featureValue)
                )
            );

            if (featureDefinition.Name == currentName)
            {
                currentCacheItem.Value = featureValue;
            }
        }

        await Cache.SetManyAsync(cacheItems, considerUow: true);
    }

    protected virtual string CalculateCacheKey(string name, string providerName, string providerKey) => FeatureValueCacheItem.CalculateCacheKey(name, providerName, providerKey);
}
