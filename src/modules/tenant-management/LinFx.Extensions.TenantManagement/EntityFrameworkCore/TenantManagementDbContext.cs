using LinFx.Extensions.Data;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.TenantManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.TenantManagement.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(TenantManagementDbProperties.ConnectionStringName)]
public class TenantManagementDbContext : EfDbContext
{
    public TenantManagementDbContext(DbContextOptions<TenantManagementDbContext> options)
        : base(options) { }

    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureTenantManagement();
    }
}
