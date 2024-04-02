using LinFx.Extensions.ObjectExtending;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.EntityFrameworkCore.ObjectExtending;

public static class EfObjectExtensionPropertyInfoExtensions
{
    public const string EfPropertyConfigurationName = "EfMapping";

    public static ObjectExtensionPropertyInfo MapEf(
        [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
    {
        Check.NotNull(propertyExtension, nameof(propertyExtension));

        propertyExtension.Configuration[EfPropertyConfigurationName] = new ObjectExtensionPropertyInfoEfMappingOptions(propertyExtension);

        return propertyExtension;
    }

    public static ObjectExtensionPropertyInfo MapEf(
        [NotNull] this ObjectExtensionPropertyInfo propertyExtension,
        [AllowNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
    {
        Check.NotNull(propertyExtension, nameof(propertyExtension));

        propertyExtension.Configuration[EfPropertyConfigurationName] =
            new ObjectExtensionPropertyInfoEfMappingOptions(
                propertyExtension,
                entityTypeAndPropertyBuildAction
            );

        return propertyExtension;
    }

    public static ObjectExtensionPropertyInfoEfMappingOptions? GetEfMappingOrNull([NotNull] this ObjectExtensionPropertyInfo propertyExtension)
    {
        Check.NotNull(propertyExtension, nameof(propertyExtension));

        return propertyExtension.Configuration.GetOrDefault(EfPropertyConfigurationName) as ObjectExtensionPropertyInfoEfMappingOptions;
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
