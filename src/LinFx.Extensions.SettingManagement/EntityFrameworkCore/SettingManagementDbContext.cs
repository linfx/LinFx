using LinFx.Data;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.SettingManagement.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(SettingManagementDbProperties.ConnectionStringName)]
    public class SettingManagementDbContext : EfCoreDbContext, ISettingManagementDbContext
    {
        public DbSet<Setting> Settings { get; set; }

        public SettingManagementDbContext(DbContextOptions<SettingManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureSettingManagement();
        }
    }
}
