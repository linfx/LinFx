using JetBrains.Annotations;
using LinFx.Extensions.ObjectExtending;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinFx.EntityFrameworkCore.ObjectExtending
{
    public static class EfCoreObjectExtensionInfoExtensions
    {
        public const string EfCoreDbContextConfigurationName = "EfCoreDbContextMapping";
        public const string EfCoreEntityConfigurationName = "EfCoreEntityMapping";

        public static ObjectExtensionInfo MapEfCoreProperty<TProperty>(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo,
            [NotNull] string propertyName,
            [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
        {
            return objectExtensionInfo.MapEfCoreProperty(
                typeof(TProperty),
                propertyName,
                entityTypeAndPropertyBuildAction
            );
        }

        public static ObjectExtensionInfo MapEfCoreProperty(
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
                    options.MapEfCore(
                        entityTypeAndPropertyBuildAction
                    );
                }
            );
        }

        public static ObjectExtensionInfo MapEfCoreEntity(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo,
            [NotNull] Action<EntityTypeBuilder> entityTypeBuildAction)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            var mappingOptionList = new List<ObjectExtensionInfoEfCoreMappingOptions>
            {
                new ObjectExtensionInfoEfCoreMappingOptions(objectExtensionInfo, entityTypeBuildAction)
            };

            objectExtensionInfo.Configuration.AddOrUpdate(EfCoreEntityConfigurationName, mappingOptionList,
                (k, v) =>
                {
                    v.As<List<ObjectExtensionInfoEfCoreMappingOptions>>().Add(mappingOptionList.First());
                    return v;
                });

            return objectExtensionInfo;
        }

        public static ObjectExtensionInfo MapEfCoreDbContext(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo,
            [NotNull] Action<ModelBuilder> modelBuildAction)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            var mappingOptionList = new List<ObjectExtensionInfoEfCoreMappingOptions>
            {
                new ObjectExtensionInfoEfCoreMappingOptions(objectExtensionInfo, modelBuildAction)
            };

            objectExtensionInfo.Configuration.AddOrUpdate(EfCoreDbContextConfigurationName, mappingOptionList, (k, v) =>
            {
                v.As<List<ObjectExtensionInfoEfCoreMappingOptions>>().Add(mappingOptionList.First());
                return v;
            });

            return objectExtensionInfo;
        }

        public static List<ObjectExtensionInfoEfCoreMappingOptions> GetEfCoreEntityMappings(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            return !objectExtensionInfo.Configuration.TryGetValue(EfCoreEntityConfigurationName, out var options) ?
                new List<ObjectExtensionInfoEfCoreMappingOptions>() : options.As<List<ObjectExtensionInfoEfCoreMappingOptions>>();
        }

        public static List<ObjectExtensionInfoEfCoreMappingOptions> GetEfCoreDbContextMappings(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            return !objectExtensionInfo.Configuration.TryGetValue(EfCoreDbContextConfigurationName, out var options) ?
                new List<ObjectExtensionInfoEfCoreMappingOptions>() : options.As<List<ObjectExtensionInfoEfCoreMappingOptions>>();
        }
    }
}
