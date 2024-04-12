using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.PermissionManagement;

public class PermissionManagementDbContext(DbContextOptions<PermissionManagementDbContext> options) : EfDbContext<PermissionManagementDbContext>(options)
{
    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigurePermissionManagement();
    }
}
