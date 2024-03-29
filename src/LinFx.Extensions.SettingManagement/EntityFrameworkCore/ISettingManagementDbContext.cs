using LinFx.Data;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.SettingManagement.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(SettingManagementDbProperties.ConnectionStringName)]
    public interface ISettingManagementDbContext : IEfCoreDbContext
    {
        DbSet<Setting> Settings { get; }
    }
}
