using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LinFx.Extensions.ObjectExtending;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinFx.Extensions.EntityFrameworkCore.ObjectExtending
{
    public static class EfObjectExtensionPropertyInfoExtensions
    {
        public const string EfPropertyConfigurationName = "EfMapping";

        [NotNull]
        public static ObjectExtensionPropertyInfo MapEf(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));

            propertyExtension.Configuration[EfPropertyConfigurationName] =
                new ObjectExtensionPropertyInfoEfMappingOptions(
                    propertyExtension
                );

            return propertyExtension;
        }

        [NotNull]
        public static ObjectExtensionPropertyInfo MapEf(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension,
            [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));

            propertyExtension.Configuration[EfPropertyConfigurationName] =
                new ObjectExtensionPropertyInfoEfMappingOptions(
                    propertyExtension,
                    entityTypeAndPropertyBuildAction
                );

            return propertyExtension;
        }

        [CanBeNull]
        public static ObjectExtensionPropertyInfoEfMappingOptions GetEfMappingOrNull(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));

            return propertyExtension
                    .Configuration
                    .GetOrDefault(EfPropertyConfigurationName)
                as ObjectExtensionPropertyInfoEfMappingOptions;
        }

        public static bool IsMappedToFieldForEf(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));

            return propertyExtension
                .Configuration
                .ContainsKey(EfPropertyConfigurationName);
        }
    }
}
