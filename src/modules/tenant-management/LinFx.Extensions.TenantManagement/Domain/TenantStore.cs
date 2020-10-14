using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.TenantManagement.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LinFx.Extensions.TenantManagement.Domain
{
    [Service]
    public class TenantStore : ITenantStore
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly TenantManagementDbContext _context;

        public TenantStore(ICurrentTenant currentTenant, TenantManagementDbContext context)
        {
            _currentTenant = currentTenant;
            _context = context;
        }

        public async Task<TenantInfo> FindAsync(string tenantIdOrName)
        {
            using (_currentTenant.Change(null))
            {
                var tenant = await _context.Tenants.FindAsync(tenantIdOrName);
                if (tenant == null)
                {
                    return null;
                }
                return new TenantInfo(tenant.Id, tenant.Name);
            }
        }
    }
}
