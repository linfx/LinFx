using LinFx.Extensions.Data;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.PermissionManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.PermissionManagement.EntityFrameworkCore;

[ConnectionStringName(PermissionManagementDbProperties.ConnectionStringName)]
public class PermissionManagementDbContext : EfDbContext, IPermissionManagementDbContext
{
    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    public PermissionManagementDbContext(DbContextOptions<PermissionManagementDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigurePermissionManagement();
    }
}
