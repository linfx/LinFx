using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.TenantManagement.EntityFrameworkCore;

namespace LinFx.Extensions.TenantManagement.Application;

[Service]
public class TenantStore(ICurrentTenant currentTenant, TenantManagementDbContext context) : ITenantStore
{
    private readonly ICurrentTenant _currentTenant = currentTenant;
    private readonly TenantManagementDbContext _context = context;

    public async Task<TenantInfo> FindAsync(string tenantIdOrName)
    {
        using (_currentTenant.Change(null))
        {
            var tenant = await _context.Tenants.FindAsync(tenantIdOrName);
            if (tenant == null)
                return null;

            return new TenantInfo(tenant.Id, tenant.Name);
        }
    }
}
