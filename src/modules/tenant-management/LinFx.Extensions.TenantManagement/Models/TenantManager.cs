using LinFx.Module.TenantManagement.Data;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LinFx.Module.TenantManagement.Models
{
    [Service]
    public class TenantManager
    {
        private readonly TenantManagementDbContext _context;

        public TenantManager(TenantManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Tenant> CreateAsync(string name)
        {
            Check.NotNull(name, nameof(name));

            await ValidateNameAsync(name);
            return new Tenant(IDUtils.NewId().ToString(), name);
        }

        protected virtual async Task ValidateNameAsync(string name, string expectedId = null)
        {
            var tenant = await _context.Tenants.FirstOrDefaultAsync(p => p.Name == name);
            if (tenant != null && tenant.Id != expectedId)
            {
                throw new UserFriendlyException("Duplicate tenancy name: " + name); //TODO: A domain exception would be better..?
            }
        }
    }
}
