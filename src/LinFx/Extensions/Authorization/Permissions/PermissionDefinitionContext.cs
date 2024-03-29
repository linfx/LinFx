using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 权限定义上下文
/// </summary>
public class PermissionDefinitionContext : IPermissionDefinitionContext
{
    /// <summary>
    /// 权限组
    /// </summary>
    public Dictionary<string, PermissionGroupDefinition> Groups { get; } = [];

    /// <summary>
    /// 增加权限组
    /// </summary>
    /// <param name="name"></param>
    /// <param name="displayName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="LinFxException"></exception>
    public virtual PermissionGroupDefinition AddGroup([NotNull] string name, LocalizedString? displayName = null)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        if (Groups.ContainsKey(name))
            throw new LinFxException($"There is already an existing permission group with name: {name}");

        return Groups[name] = new PermissionGroupDefinition(name, displayName);
    }

    /// <summary>
    /// 获取权限组
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual PermissionGroupDefinition? GetGroupOrNull(string name)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        if (!Groups.ContainsKey(name))
            return null;

        return Groups[name];
    }
}
