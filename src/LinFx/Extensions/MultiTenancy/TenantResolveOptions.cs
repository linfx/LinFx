using System.Collections.Generic;

namespace LinFx.Extensions.MultiTenancy
{
    public class TenantResolveOptions
    {
        [NotNull]
        public List<ITenantResolveContributor> TenantResolvers { get; }

        public TenantResolveOptions()
        {
            TenantResolvers = new List<ITenantResolveContributor>
            {
                new CurrentUserTenantResolveContributor(),
                new HeaderTenantResolveContributor()
            };
        }
    }
}