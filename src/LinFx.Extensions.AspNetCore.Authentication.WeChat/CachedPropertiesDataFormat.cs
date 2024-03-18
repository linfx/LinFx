using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Caching.Distributed;

namespace Microsoft.AspNetCore.Authentication.Wechat;

/// <summary>
/// 自定义缓存PropertiesDataFormat
/// https://github.com/IdentityServer/IdentityServer4/issues/407
/// </summary>
public class CachedPropertiesDataFormat(IDistributedCache cache, IDataProtector dataProtector, IDataSerializer<AuthenticationProperties> serializer) : ISecureDataFormat<AuthenticationProperties>
{
    public const string CacheKeyPrefix = "CachedPropertiesData-";

    private readonly IDistributedCache _cache = cache;
    private readonly IDataProtector _dataProtector = dataProtector;
    private readonly IDataSerializer<AuthenticationProperties> _serializer = serializer;

    public CachedPropertiesDataFormat(IDistributedCache cache, IDataProtector dataProtector)
        : this(cache, dataProtector, new PropertiesSerializer()) { }

    public string Protect(AuthenticationProperties data) => Protect(data, null);

    public string Protect(AuthenticationProperties data, string? purpose)
    {
        var key = Guid.NewGuid().ToString();
        var cacheKey = $"{CacheKeyPrefix}{key}";
        var serialized = _serializer.Serialize(data);

        // Rather than encrypt the full AuthenticationProperties
        // cache the data and encrypt the key that points to the data
        _cache.Set(cacheKey, serialized);

        return key;
    }

    public AuthenticationProperties Unprotect(string protectedText) => Unprotect(protectedText, null);

    public AuthenticationProperties Unprotect(string protectedText, string? purpose)
    {
        // Decrypt the key and retrieve the data from the cache.
        var key = protectedText;
        var cacheKey = $"{CacheKeyPrefix}{key}";
        var serialized = _cache.Get(cacheKey);

        return _serializer.Deserialize(serialized);
    }
}
