﻿using Microsoft.Extensions.Localization;
using System.Collections.Immutable;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 权限定义
/// </summary>
public class PermissionDefinition
{
    private readonly List<PermissionDefinition> _children;

    /// <summary>
    /// 唯一的权限标识名称。
    /// Unique name of the permission.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 当前权限的父级权限。
    /// Parent of this permission if one exists.
    /// If set, this permission can be granted only if parent is granted.
    /// </summary>
    public PermissionDefinition? Parent { get; private set; }

    /// <summary>
    /// A list of allowed providers to get/set value of this permission.
    /// An empty list indicates that all providers are allowed.
    /// </summary>
    public List<string> Providers { get; }

    /// <summary>
    /// 权限的多语言名称。
    /// </summary>
    public LocalizedString? DisplayName { get; set; }

    /// <summary>
    /// 子项权限
    /// </summary>
    public IReadOnlyList<PermissionDefinition> Children => _children.ToImmutableList();

    /// <summary>
    /// Can be used to get/set custom properties for this permission definition.
    /// </summary>
    public Dictionary<string, object> Properties { get; }

    /// <summary>
    /// Indicates whether this permission is enabled or disabled.
    /// A permission is normally enabled.
    /// A disabled permission can not be granted to anyone, but it is still
    /// will be available to check its value (while it will always be false).
    ///
    /// Disabling a permission would be helpful to hide a related application
    /// functionality from users/clients.
    ///
    /// Default: true.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Gets/sets a key-value on the <see cref="Properties"/>.
    /// </summary>
    /// <param name="name">Name of the property</param>
    /// <returns>
    /// Returns the value in the <see cref="Properties"/> dictionary by given name.
    /// Returns null if given name is not present in the <see cref="Properties"/> dictionary.
    /// </returns>
    public object this[string name]
    {
        get => Properties.GetOrDefault(name)!;
        set => Properties[name] = value;
    }

    protected internal PermissionDefinition(string name, LocalizedString? displayName = null)
    {
        Name = Check.NotNull(name, nameof(name));
        DisplayName = displayName;

        Properties = [];
        Providers = [];
        _children = [];
    }

    public virtual PermissionDefinition AddChild(string name, LocalizedString? displayName = null)
    {
        var child = new PermissionDefinition(name, displayName) { Parent = this };
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

    public override string ToString() => $"[{nameof(PermissionDefinition)} {Name}]";
}
