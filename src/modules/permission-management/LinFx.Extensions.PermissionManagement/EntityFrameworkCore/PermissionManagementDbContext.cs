using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.PermissionManagement.EntityFrameworkCore
{
    public class PermissionManagementDbContext : LinFx.EntityFrameworkCore.DbContext
    {
        public PermissionManagementDbContext(DbContextOptions<PermissionManagementDbContext> options)
            : base(options) { }

        /// <summary>
        /// 权限授权
        /// </summary>
        public DbSet<PermissionGrant> PermissionGrants { get; set; }
    }
}
