using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.PermissionManagement.EntityFrameworkCore;

public class PermissionManagementDbContext : EfCoreDbContext, IPermissionManagementDbContext
{
    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    public PermissionManagementDbContext(DbContextOptions<PermissionManagementDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //builder.ConfigurePermissionManagement();
    }
}
