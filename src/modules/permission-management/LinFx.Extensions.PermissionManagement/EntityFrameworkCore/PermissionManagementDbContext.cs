using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.PermissionManagement.EntityFrameworkCore;

public class PermissionManagementDbContext : DbContext
{
    public PermissionManagementDbContext() { }

    public PermissionManagementDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigurePermissionManagement();
    }
}
