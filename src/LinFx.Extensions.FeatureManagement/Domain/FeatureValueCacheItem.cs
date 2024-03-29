using LinFx.Extensions.MultiTenancy;

namespace LinFx.Extensions.FeatureManagement;

[Serializable]
[IgnoreMultiTenancy]
public class FeatureValueCacheItem
{
    public string Value { get; set; } = string.Empty;

    public FeatureValueCacheItem() { }

    public FeatureValueCacheItem(string value) => Value = value;

    public static string CalculateCacheKey(string name, string providerName, string providerKey) => "pn:" + providerName + ",pk:" + providerKey + ",n:" + name;
}
