using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.PermissionManagement;

public class PermissionManagementDbContext : EfDbContext<PermissionManagementDbContext>
{
    public PermissionManagementDbContext() { }

    public PermissionManagementDbContext(DbContextOptions<PermissionManagementDbContext> options) : base(options) { }

    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigurePermissionManagement();
    }
}
