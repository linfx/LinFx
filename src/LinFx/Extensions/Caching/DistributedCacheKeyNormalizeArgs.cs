namespace LinFx.Extensions.Caching;

public class DistributedCacheKeyNormalizeArgs(string key, string cacheName, bool ignoreMultiTenancy)
{
    public string Key { get; } = key;

    public string CacheName { get; } = cacheName;

    public bool IgnoreMultiTenancy { get; } = ignoreMultiTenancy;
}