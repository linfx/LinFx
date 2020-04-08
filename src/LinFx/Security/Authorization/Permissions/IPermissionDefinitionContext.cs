using System.Diagnostics.CodeAnalysis;

namespace LinFx.Security.Authorization.Permissions
{
    public interface IPermissionDefinitionContext
    {
        PermissionGroupDefinition GetGroupOrNull(string name);

        PermissionGroupDefinition AddGroup([NotNull] string name, string displayName = null);
    }
}