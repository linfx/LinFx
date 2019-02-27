using System;
using System.Threading.Tasks;
using LinFx.Extensions.MultiTenancy;

namespace LinFx.Extensions.AspNetCore.MultiTenancy
{
    internal interface ITenantStore
    {
        Task<TenantInfo> FindAsync(Guid parsedTenantId);
        Task<TenantInfo> FindAsync(string tenantIdOrName);
    }
}