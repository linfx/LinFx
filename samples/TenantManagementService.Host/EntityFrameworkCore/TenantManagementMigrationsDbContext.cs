using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.TenantManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TenantManagementService.EntityFrameworkCore;

public class TenantManagementMigrationsDbContext : EfDbContext
{
    public TenantManagementMigrationsDbContext(DbContextOptions<TenantManagementMigrationsDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureTenantManagement();
    }
}
