using JetBrains.Annotations;
using LinFx.Extensions.ObjectExtending;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinFx.Extensions.EntityFrameworkCore.ObjectExtending;

public static class EfObjectExtensionInfoExtensions
{
    public const string EfDbContextConfigurationName = "EfDbContextMapping";
    public const string EfEntityConfigurationName = "EfEntityMapping";

    public static ObjectExtensionInfo MapEfProperty<TProperty>(
        [NotNull] this ObjectExtensionInfo objectExtensionInfo,
        [NotNull] string propertyName,
        [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
    {
        return objectExtensionInfo.MapEfProperty(
            typeof(TProperty),
            propertyName,
            entityTypeAndPropertyBuildAction
        );
    }

    public static ObjectExtensionInfo MapEfProperty(
        [NotNull] this ObjectExtensionInfo objectExtensionInfo,
        [NotNull] Type propertyType,
        [NotNull] string propertyName,
        [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
    {
        Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

        return objectExtensionInfo.AddOrUpdateProperty(
            propertyType,
            propertyName,
            options =>
            {
                options.MapEf(
                    entityTypeAndPropertyBuildAction
                );
            }
        );
    }

    public static ObjectExtensionInfo MapEfEntity(
        [NotNull] this ObjectExtensionInfo objectExtensionInfo,
        [NotNull] Action<EntityTypeBuilder> entityTypeBuildAction)
    {
        Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

        var mappingOptionList = new List<ObjectExtensionInfoEfMappingOptions>
        {
            new(objectExtensionInfo, entityTypeBuildAction)
        };

        objectExtensionInfo.Configuration.AddOrUpdate(EfEntityConfigurationName, mappingOptionList, (k, v) =>
        {
            v.As<List<ObjectExtensionInfoEfMappingOptions>>().Add(mappingOptionList.First());
            return v;
        });

        return objectExtensionInfo;
    }

    public static ObjectExtensionInfo MapEfeDbContext(
        [NotNull] this ObjectExtensionInfo objectExtensionInfo,
        [NotNull] Action<ModelBuilder> modelBuildAction)
    {
        Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

        var mappingOptionList = new List<ObjectExtensionInfoEfMappingOptions>
        {
            new(objectExtensionInfo, modelBuildAction)
        };

        objectExtensionInfo.Configuration.AddOrUpdate(EfDbContextConfigurationName, mappingOptionList, (k, v) =>
        {
            v.As<List<ObjectExtensionInfoEfMappingOptions>>().Add(mappingOptionList.First());
            return v;
        });

        return objectExtensionInfo;
    }

    public static List<ObjectExtensionInfoEfMappingOptions> GetEfEntityMappings(
        [NotNull] this ObjectExtensionInfo objectExtensionInfo)
    {
        Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

        return !objectExtensionInfo.Configuration.TryGetValue(EfEntityConfigurationName, out var options) ? [] : options.As<List<ObjectExtensionInfoEfMappingOptions>>();
    }

    public static List<ObjectExtensionInfoEfMappingOptions> GetEfDbContextMappings(
        [NotNull] this ObjectExtensionInfo objectExtensionInfo)
    {
        Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

        return !objectExtensionInfo.Configuration.TryGetValue(EfDbContextConfigurationName, out var options) ? [] : options.As<List<ObjectExtensionInfoEfMappingOptions>>();
    }
}
