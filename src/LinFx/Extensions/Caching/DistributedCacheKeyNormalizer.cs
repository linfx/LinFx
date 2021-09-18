using LinFx.Extensions.MultiTenancy;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.Caching
{
    [Service]
    public class DistributedCacheKeyNormalizer : IDistributedCacheKeyNormalizer
    {
        protected ICurrentTenant CurrentTenant { get; }

        protected DistributedCacheOptions DistributedCacheOptions { get; }

        public DistributedCacheKeyNormalizer(
            ICurrentTenant currentTenant,
            IOptions<DistributedCacheOptions> distributedCacheOptions)
        {
            CurrentTenant = currentTenant;
            DistributedCacheOptions = distributedCacheOptions.Value;
        }

        public virtual string NormalizeKey(DistributedCacheKeyNormalizeArgs args)
        {
            var normalizedKey = $"c:{args.CacheName},k:{DistributedCacheOptions.KeyPrefix}{args.Key}";

            if (!args.IgnoreMultiTenancy && string.IsNullOrEmpty(CurrentTenant.Id))
            {
                normalizedKey = $"t:{CurrentTenant.Id},{normalizedKey}";
            }

            return normalizedKey;
        }
    }
}