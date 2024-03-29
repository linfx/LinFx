using JetBrains.Annotations;
using LinFx.Extensions.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.FeatureManagement;

public static class FeatureManagementDbContextModelBuilderExtensions
{
    public static void ConfigureFeatureManagement([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<FeatureValue>(b =>
        {
            b.ToTable(FeatureManagementDbProperties.DbTablePrefix + nameof(FeatureValue), FeatureManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //b.Property(x => x.Name).HasMaxLength(FeatureValueConsts.MaxNameLength).IsRequired();
            //b.Property(x => x.Value).HasMaxLength(FeatureValueConsts.MaxValueLength).IsRequired();
            //b.Property(x => x.ProviderName).HasMaxLength(FeatureValueConsts.MaxProviderNameLength);
            //b.Property(x => x.ProviderKey).HasMaxLength(FeatureValueConsts.MaxProviderKeyLength);

            b.HasIndex(x => new { x.Name, x.ProviderName, x.ProviderKey }).IsUnique();

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<FeatureManagementDbContext>();
    }
}
