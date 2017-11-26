using LinFx.SaaS.Features;

namespace LinFx.SaaS.MultiTenancy
{
    public class TenantFeatureSetting : FeatureSetting
    {
        /// <summary>
        /// Tenant's Id.
        /// </summary>
        public virtual int TenantId { get; set; }
    }
}
