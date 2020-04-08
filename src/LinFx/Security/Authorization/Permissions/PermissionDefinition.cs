using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Security.Authorization.Permissions
{
    public class PermissionDefinition
    {
        /// <summary>
        /// Unique name of the permission.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Parent of this permission if one exists.
        /// If set, this permission can be granted only if parent is granted.
        /// </summary>
        public PermissionDefinition Parent { get; private set; }

        /// <summary>
        /// A list of allowed providers to get/set value of this permission.
        /// An empty list indicates that all providers are allowed.
        /// </summary>
        public List<string> Providers { get; }

        public string DisplayName
        {
            get => _displayName;
            //set => _displayName = Check.NotNull(value, nameof(value));
            set => _displayName = value;
        }
        private string _displayName;

        public IReadOnlyList<PermissionDefinition> Children => _children.ToImmutableList();
        private readonly List<PermissionDefinition> _children;

        /// <summary>
        /// Can be used to get/set custom properties for this permission definition.
        /// </summary>
        public Dictionary<string, object> Properties { get; }

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

        protected internal PermissionDefinition(
            [NotNull] string name,
            string displayName = null)
        {
            Name = Check.NotNull(name, nameof(name));
            DisplayName = displayName;

            Properties = new Dictionary<string, object>();
            Providers = new List<string>();
            _children = new List<PermissionDefinition>();
        }

        public virtual PermissionDefinition AddChild(
            [NotNull] string name,
            string displayName = null)
        {
            var child = new PermissionDefinition(name, displayName)
            {
                Parent = this
            };

            _children.Add(child);

            return child;
        }

        /// <summary>
        /// Sets a property in the <see cref="Properties"/> dictionary.
        /// This is a shortcut for nested calls on this object.
        /// </summary>
        public virtual PermissionDefinition WithProperty(string key, object value)
        {
            Properties[key] = value;
            return this;
        }

        /// <summary>
        /// Sets a property in the <see cref="Properties"/> dictionary.
        /// This is a shortcut for nested calls on this object.
        /// </summary>
        public virtual PermissionDefinition WithProviders(params string[] providers)
        {
            if (!providers.IsNullOrEmpty())
            {
                Providers.AddRange(providers);
            }

            return this;
        }

        public override string ToString()
        {
            return $"[{nameof(PermissionDefinition)} {Name}]";
        }
    }
}
