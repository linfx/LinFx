using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.TenantManagement;

[IgnoreMultiTenancy]
public class TenantManagementDbContext(DbContextOptions options) : EfDbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureTenantManagement();
    }
}
