using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Authorization.Permissions
{
    /// <summary>
    /// 权限管理器
    /// </summary>
    public interface IPermissionDefinitionManager
    {
        /// <summary>
        /// 根据权限定义的唯一标识获取权限。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        PermissionDefinition Get([NotNull] string name);

        /// <summary>
        /// 根据权限定义的唯一标识获取权限，如果权限不存在，则返回 null。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        PermissionDefinition GetOrNull([NotNull] string name);

        /// <summary>
        /// 获取所有权限。
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<PermissionDefinition> GetPermissions();

        /// <summary>
        /// 获取所有权限组。
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<PermissionGroupDefinition> GetGroups();
    }
}