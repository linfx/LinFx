using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace LinFx.Extensions.Authorization.Permissions
{
    /// <summary>
    /// 权限定义上下文
    /// </summary>
    [Service(ServiceLifetime.Singleton)]
    public class PermissionDefinitionContext : IPermissionDefinitionContext
    {
        /// <summary>
        /// 权限组
        /// </summary>
        public Dictionary<string, PermissionGroupDefinition> Groups { get; } = new Dictionary<string, PermissionGroupDefinition>();

        public virtual PermissionGroupDefinition AddGroup(string name, string displayName = null)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (Groups.ContainsKey(name))
                throw new LinFxException($"There is already an existing permission group with name: {name}");

            return Groups[name] = new PermissionGroupDefinition(name, displayName);
        }

        public virtual PermissionGroupDefinition GetGroupOrNull(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (!Groups.ContainsKey(name))
                return null;

            return Groups[name];
        }

    }
}