using Microsoft.Extensions.Localization;

namespace LinFx.Security.Authorization.Permissions
{
    public interface IPermissionDefinitionContext
    {
        PermissionGroupDefinition GetGroupOrNull(string name);

        PermissionGroupDefinition AddGroup([NotNull] string name, IStringLocalizer displayName = null);
    }
}