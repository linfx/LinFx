using System;
using LinFx.Extensions.MultiTenancy;

namespace LinFx.Identity
{
    /// <summary>
    /// Represents a role in the identity system
    /// </summary>
    public class IdentityRole : Microsoft.AspNetCore.Identity.IdentityRole, IMultiTenant
    {
        public Guid? TenantId { set; get; }
    }
}