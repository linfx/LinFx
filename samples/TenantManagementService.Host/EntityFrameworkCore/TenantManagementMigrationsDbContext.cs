using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.TenantManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TenantManagementService.EntityFrameworkCore;

public class TenantManagementMigrationsDbContext : EfDbContext
{
    protected TenantManagementMigrationsDbContext() { }

    public TenantManagementMigrationsDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureTenantManagement();
    }
}
