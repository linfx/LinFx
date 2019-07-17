using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace LinFx.Security.Authorization.Permissions
{
    /// <summary>
    /// 权限组
    /// </summary>
    //TODO: Consider to make possible a group have sub groups
    public class PermissionGroupDefinition 
    {
        /// <summary>
        /// Unique name of the group.
        /// </summary>
        public string Name { get; }

        public Dictionary<string, object> Properties { get; }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            //set => _displayName = Check.NotNull(value, nameof(value));
            set => _displayName = value;
        }

        private readonly List<PermissionDefinition> _permissions;
        public IReadOnlyList<PermissionDefinition> Permissions => _permissions.ToImmutableList();

#pragma warning disable CS1574 // XML 注释中有未能解析的 cref 特性“name”
#pragma warning disable CS1574 // XML 注释中有未能解析的 cref 特性“name”
        /// <summary>
        /// Gets/sets a key-value on the <see cref="Properties"/>.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <returns>
        /// Returns the value in the <see cref="Properties"/> dictionary by given <see cref="name"/>.
        /// Returns null if given <see cref="name"/> is not present in the <see cref="Properties"/> dictionary.
        /// </returns>
        public object this[string name]
#pragma warning restore CS1574 // XML 注释中有未能解析的 cref 特性“name”
#pragma warning restore CS1574 // XML 注释中有未能解析的 cref 特性“name”
        {
            get => Properties.GetOrDefault(name);
            set => Properties[name] = value;
        }

        protected internal PermissionGroupDefinition(string name, string displayName = null)
        {
            Name = name;
            DisplayName = displayName;

            Properties = new Dictionary<string, object>();
            _permissions = new List<PermissionDefinition>();
        }

        public virtual PermissionDefinition AddPermission(string name, string displayName = null)
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