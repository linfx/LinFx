using System;
using LinFx.Extensions.MultiTenancy;

namespace LinFx.Identity
{
    /// <summary>
    /// Represents a user in the identity system
    /// </summary>
    public class IdentityUser : Microsoft.AspNetCore.Identity.IdentityUser, IMultiTenant
    {
        public Guid? TenantId { set; get; }
    }
}
