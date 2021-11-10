using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace LinFx.Extensions.Authorization.Permissions
{
    /// <summary>
    /// 权限组定义
    /// </summary>
    public class PermissionGroupDefinition
    {
        private readonly List<PermissionDefinition> _permissions;

        /// <summary>
        /// 唯一的权限组标识名称。
        /// Unique name of the group.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 权限组的一些自定义属性。
        /// </summary>
        public Dictionary<string, object> Properties { get; }

        /// <summary>
        /// 本地化名称。
        /// </summary>
        public LocalizedString DisplayName { get; set; }

        /// <summary>
        /// 权限组下面的所属权限。
        /// </summary>
        public IReadOnlyList<PermissionDefinition> Permissions => _permissions.ToImmutableList();

        /// <summary>
        /// 自定义属性的快捷索引器。
        /// Gets/sets a key-value on the <see cref="Properties"/>.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <returns>
        /// Returns the value in the <see cref="Properties"/> dictionary by given name.
        /// Returns null if given name is not present in the <see cref="Properties"/> dictionary.
        /// </returns>
        public object this[string name]
        {
            get => Properties.GetOrDefault(name);
            set => Properties[name] = value;
        }

        protected internal PermissionGroupDefinition(string name, LocalizedString displayName = null)
        {
            Name = name;
            DisplayName = displayName;

            Properties = new Dictionary<string, object>();
            _permissions = new List<PermissionDefinition>();
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public virtual PermissionDefinition AddPermission(string name, LocalizedString displayName = null)
        {
            var permission = new PermissionDefinition(name, displayName);
            _permissions.Add(permission);
            return permission;
        }

        public virtual List<PermissionDefinition> GetPermissionsWithChildren()
        {
            var permissions = new List<PermissionDefinition>();

            foreach (var permission in _permissions)
            {
                AddPermissionToListRecursively(permissions, permission);
            }

            return permissions;
        }

        private void AddPermissionToListRecursively(List<PermissionDefinition> permissions, PermissionDefinition permission)
        {
            permissions.Add(permission);

            foreach (var child in permission.Children)
            {
                AddPermissionToListRecursively(permissions, child);
            }
        }

        public override string ToString()
        {
            return $"[{nameof(PermissionGroupDefinition)} {Name}]";
        }
    }
}