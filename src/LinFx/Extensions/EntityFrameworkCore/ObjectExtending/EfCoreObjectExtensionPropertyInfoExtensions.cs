﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LinFx.Extensions.ObjectExtending;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinFx.Extensions.EntityFrameworkCore.ObjectExtending
{
    public static class EfCoreObjectExtensionPropertyInfoExtensions
    {
        public const string EfCorePropertyConfigurationName = "EfCoreMapping";

        [NotNull]
        public static ObjectExtensionPropertyInfo MapEfCore(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));

            propertyExtension.Configuration[EfCorePropertyConfigurationName] =
                new ObjectExtensionPropertyInfoEfCoreMappingOptions(
                    propertyExtension
                );

            return propertyExtension;
        }

        [NotNull]
        public static ObjectExtensionPropertyInfo MapEfCore(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension,
            [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));

            propertyExtension.Configuration[EfCorePropertyConfigurationName] =
                new ObjectExtensionPropertyInfoEfCoreMappingOptions(
                    propertyExtension,
                    entityTypeAndPropertyBuildAction
                );

            return propertyExtension;
        }

        [CanBeNull]
        public static ObjectExtensionPropertyInfoEfCoreMappingOptions GetEfCoreMappingOrNull(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));

            return propertyExtension
                    .Configuration
                    .GetOrDefault(EfCorePropertyConfigurationName)
                as ObjectExtensionPropertyInfoEfCoreMappingOptions;
        }

        public static bool IsMappedToFieldForEfCore(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));

            return propertyExtension
                .Configuration
                .ContainsKey(EfCorePropertyConfigurationName);
        }
    }
}