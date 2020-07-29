using System.Diagnostics.CodeAnalysis;

namespace LinFx.Security.Authorization.Permissions
{
    /// <summary>
    /// 权限定义上下文
    /// </summary>
    public interface IPermissionDefinitionContext
    {
        PermissionGroupDefinition GetGroupOrNull(string name);

        PermissionGroupDefinition AddGroup([NotNull] string name, string displayName = null);
    }
}