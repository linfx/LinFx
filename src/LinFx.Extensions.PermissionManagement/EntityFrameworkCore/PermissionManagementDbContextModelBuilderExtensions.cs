using LinFx.Extensions.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.PermissionManagement.EntityFrameworkCore;

public static class PermissionManagementDbContextModelBuilderExtensions
{
    public static void ConfigurePermissionManagement(this ModelBuilder builder)
    {
        builder.Entity<PermissionGrant>(b =>
        {
            b.ToTable(PermissionManagementDbProperties.DbTablePrefix + "PermissionGrants", PermissionManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //b.Property(x => x.Name).HasMaxLength(PermissionGrantConsts.MaxNameLength).IsRequired();
            //b.Property(x => x.ProviderName).HasMaxLength(PermissionGrantConsts.MaxProviderNameLength).IsRequired();
            //b.Property(x => x.ProviderKey).HasMaxLength(PermissionGrantConsts.MaxProviderKeyLength).IsRequired();

            b.HasIndex(x => new { x.TenantId, x.Name, x.ProviderName, x.ProviderKey }).IsUnique(true);

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<PermissionManagementDbContext>();
    }
}
