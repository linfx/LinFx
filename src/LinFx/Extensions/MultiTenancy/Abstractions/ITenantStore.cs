using System;
using System.Threading.Tasks;
using LinFx.Extensions.MultiTenancy;

namespace LinFx.Extensions.AspNetCore.MultiTenancy
{
    internal interface ITenantStore
    {
        Task<Tenant> FindAsync(Guid parsedTenantId);

        Task<Tenant> FindAsync(string tenantIdOrName);
    }
}