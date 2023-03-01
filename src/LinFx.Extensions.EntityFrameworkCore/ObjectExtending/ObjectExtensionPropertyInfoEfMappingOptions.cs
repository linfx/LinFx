using JetBrains.Annotations;
using LinFx.Extensions.ObjectExtending;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinFx.Extensions.EntityFrameworkCore.ObjectExtending;

public class ObjectExtensionPropertyInfoEfMappingOptions
{
    [NotNull]
    public ObjectExtensionPropertyInfo ExtensionProperty { get; }

    [NotNull]
    public ObjectExtensionInfo ObjectExtension => ExtensionProperty.ObjectExtension;

    [CanBeNull]
    [Obsolete("Use EntityTypeAndPropertyBuildAction property.")]
    public Action<PropertyBuilder> PropertyBuildAction { get; set; }

    [CanBeNull]
    public Action<EntityTypeBuilder, PropertyBuilder> EntityTypeAndPropertyBuildAction { get; set; }

    public ObjectExtensionPropertyInfoEfMappingOptions(
        [NotNull] ObjectExtensionPropertyInfo extensionProperty)
    {
        ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));
    }

    public ObjectExtensionPropertyInfoEfMappingOptions(
        [NotNull] ObjectExtensionPropertyInfo extensionProperty,
        [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
    {
        ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));

        EntityTypeAndPropertyBuildAction = entityTypeAndPropertyBuildAction;
    }
}
