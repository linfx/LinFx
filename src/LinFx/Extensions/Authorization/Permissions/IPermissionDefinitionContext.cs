using Microsoft.Extensions.Localization;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 权限定义上下文
/// </summary>
public interface IPermissionDefinitionContext
{
    /// <summary>
    /// 获取权限组
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    PermissionGroupDefinition? GetGroupOrNull(string name);

    /// <summary>
    /// 添加权限组
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="displayName">显示名称</param>
    /// <returns></returns>
    PermissionGroupDefinition AddGroup(string name, LocalizedString? displayName = null);
}
