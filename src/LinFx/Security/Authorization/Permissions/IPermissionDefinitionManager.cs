using System.Collections.Generic;

namespace LinFx.Security.Authorization.Permissions
{
    public interface IPermissionDefinitionManager
    {
        [NotNull]
        PermissionDefinition Get([NotNull] string name);

        [CanBeNull]
        PermissionDefinition GetOrNull([NotNull] string name);

        IReadOnlyList<PermissionDefinition> GetPermissions();

        IReadOnlyList<PermissionGroupDefinition> GetGroups();
    }
}