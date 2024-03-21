using LinFx.Extensions.EntityFrameworkCore.Modeling;
using LinFx.Extensions.TenantManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.TenantManagement.EntityFrameworkCore;

public static class TenantManagementDbContextModelCreatingExtensions
{
    public static void ConfigureTenantManagement(this ModelBuilder builder)
    {
        //if (builder.IsTenantOnlyDatabase())
        //{
        //    return;
        //}

        builder.Entity<Tenant>(b =>
        {
            b.ToTable(TenantManagementDbProperties.DbTablePrefix + "Tenants", TenantManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //b.Property(t => t.Name).IsRequired().HasMaxLength(TenantConsts.MaxNameLength);

            //b.HasMany(u => u.ConnectionStrings).WithOne().HasForeignKey(uc => uc.TenantId).IsRequired();

            b.HasIndex(u => u.Name);

            b.ApplyObjectExtensionMappings();
        });

        //builder.Entity<TenantConnectionString>(b =>
        //{
        //    b.ToTable(AbpTenantManagementDbProperties.DbTablePrefix + "TenantConnectionStrings", AbpTenantManagementDbProperties.DbSchema);

        //    b.ConfigureByConvention();

        //    b.HasKey(x => new { x.TenantId, x.Name });

        //    b.Property(cs => cs.Name).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxNameLength);
        //    b.Property(cs => cs.Value).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxValueLength);

        //    b.ApplyObjectExtensionMappings();
        //});

        builder.TryConfigureObjectExtensions<TenantManagementDbContext>();
    }
}
