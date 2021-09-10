using System;
using JetBrains.Annotations;
using LinFx.Extensions.ObjectExtending;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinFx.EntityFrameworkCore.ObjectExtending
{
    public class ObjectExtensionPropertyInfoEfCoreMappingOptions
    {
        [NotNull]
        public ObjectExtensionPropertyInfo ExtensionProperty { get; }

        [NotNull]
        public ObjectExtensionInfo ObjectExtension => ExtensionProperty.ObjectExtension;

        [Obsolete("Use EntityTypeAndPropertyBuildAction property.")]
        [CanBeNull]
        public Action<PropertyBuilder> PropertyBuildAction { get; set; }

        [CanBeNull]
        public Action<EntityTypeBuilder, PropertyBuilder> EntityTypeAndPropertyBuildAction { get; set; }

        public ObjectExtensionPropertyInfoEfCoreMappingOptions(
            [NotNull] ObjectExtensionPropertyInfo extensionProperty)
        {
            ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));
        }

        public ObjectExtensionPropertyInfoEfCoreMappingOptions(
            [NotNull] ObjectExtensionPropertyInfo extensionProperty,
            [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
        {
            ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));

            EntityTypeAndPropertyBuildAction = entityTypeAndPropertyBuildAction;
        }
    }
}
