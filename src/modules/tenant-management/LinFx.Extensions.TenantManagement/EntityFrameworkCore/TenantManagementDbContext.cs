using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.TenantManagement.EntityFrameworkCore;

[IgnoreMultiTenancy]
public class TenantManagementDbContext : EfDbContext
{
    public TenantManagementDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureTenantManagement();
    }
}
