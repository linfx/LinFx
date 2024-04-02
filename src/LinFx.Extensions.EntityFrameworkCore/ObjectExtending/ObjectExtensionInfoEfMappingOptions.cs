using System.Diagnostics.CodeAnalysis;
using LinFx.Extensions.ObjectExtending;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinFx.Extensions.EntityFrameworkCore.ObjectExtending;

public class ObjectExtensionInfoEfMappingOptions
{
    [NotNull]
    public ObjectExtensionInfo ObjectExtension { get; }

    [AllowNull]
    public Action<EntityTypeBuilder> EntityTypeBuildAction { get; set; }

    [AllowNull]
    public Action<ModelBuilder> ModelBuildAction { get; set; }

    public ObjectExtensionInfoEfMappingOptions(
        [NotNull] ObjectExtensionInfo objectExtension,
        [NotNull] Action<EntityTypeBuilder> entityTypeBuildAction)
    {
        ObjectExtension = Check.NotNull(objectExtension, nameof(objectExtension));
        EntityTypeBuildAction = Check.NotNull(entityTypeBuildAction, nameof(entityTypeBuildAction));

        EntityTypeBuildAction = entityTypeBuildAction;
    }

    public ObjectExtensionInfoEfMappingOptions(
        [NotNull] ObjectExtensionInfo objectExtension,
        [NotNull] Action<ModelBuilder> modelBuildAction)
    {
        ObjectExtension = Check.NotNull(objectExtension, nameof(objectExtension));
        ModelBuildAction = Check.NotNull(modelBuildAction, nameof(modelBuildAction));

        ModelBuildAction = modelBuildAction;
    }
}
