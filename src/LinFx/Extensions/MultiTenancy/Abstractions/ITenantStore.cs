using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.MultiTenancy
{
    public interface ITenantStore
    {
        Task<TenantInfo> FindAsync(Guid parsedTenantId);

        Task<TenantInfo> FindAsync(string tenantIdOrName);
    }
}