using LinFx.Extensions.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.TenantManagement.EntityFrameworkCore;

[IgnoreMultiTenancy]
public class TenantManagementDbContext : DbContext
{
    public TenantManagementDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureTenantManagement();
    }
}
