using JetBrains.Annotations;
using LinFx.Extensions.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using System;

namespace LinFx.Extensions.SettingManagement.EntityFrameworkCore
{
    public class SettingManagementModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public SettingManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema)
            : base(
                tablePrefix,
                schema)
        {
        }
    }

    public static class SettingManagementDbContextModelBuilderExtensions
    {
        //TODO: Instead of getting parameters, get a action of SettingManagementModelBuilderConfigurationOptions like other modules
        public static void ConfigureSettingManagement(
            [NotNull] this ModelBuilder builder,
            [CanBeNull] Action<SettingManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new SettingManagementModelBuilderConfigurationOptions(
                SettingManagementDbProperties.DbTablePrefix,
                SettingManagementDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<Setting>(b =>
            {
                b.ToTable(options.TablePrefix + "Settings", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).HasMaxLength(SettingConsts.MaxNameLength).IsRequired();

                if (builder.IsUsingOracle()) { SettingConsts.MaxValueLengthValue = 2000; }
                b.Property(x => x.Value).HasMaxLength(SettingConsts.MaxValueLengthValue).IsRequired();

                b.Property(x => x.ProviderName).HasMaxLength(SettingConsts.MaxProviderNameLength);
                b.Property(x => x.ProviderKey).HasMaxLength(SettingConsts.MaxProviderKeyLength);

                b.HasIndex(x => new { x.Name, x.ProviderName, x.ProviderKey });

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<SettingManagementDbContext>();
        }
    }
}
