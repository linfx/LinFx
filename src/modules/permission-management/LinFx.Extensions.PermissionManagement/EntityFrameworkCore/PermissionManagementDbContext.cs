using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.PermissionManagement.EntityFrameworkCore
{
    public class PermissionManagementDbContext : EfCodeDbContext, IPermissionManagementDbContext
    {
        public PermissionManagementDbContext(DbContextOptions<PermissionManagementDbContext> options)
            : base(options) { }

        public DbSet<PermissionGrant> PermissionGrants { get; set; }
    }
}
