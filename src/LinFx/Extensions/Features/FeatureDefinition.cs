using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace LinFx.Extensions.Features;

/// <summary>
/// 特征定义
/// </summary>
public class FeatureDefinition
{
    private readonly List<FeatureDefinition>? _children;

    /// <summary>
    /// Unique name of the feature.
    /// </summary>
    public string Name { get; } = string.Empty;

    public LocalizableString DisplayName { get; set; }

    /// <summary>
    /// Default value of the feature.
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public LocalizableString? Description { get; set; }

    /// <summary>
    /// Parent of this feature, if one exists.
    /// If set, this feature can be enabled only if the parent is enabled.
    /// </summary>
    public FeatureDefinition? Parent { get; private set; }

    /// <summary>
    /// List of child features.
    /// </summary>
    public IReadOnlyList<FeatureDefinition> Children => _children.ToImmutableList();

    /// <summary>
    /// Gets/sets a key-value on the <see cref="Properties"/>.
    /// </summary>
    /// <param name="name">Name of the property</param>
    /// <returns>
    /// Returns the value in the <see cref="Properties"/> dictionary by given <paramref name="name"/>.
    /// Returns null if given <paramref name="name"/> is not present in the <see cref="Properties"/> dictionary.
    /// </returns>
    public object? this[string name]
    {
        get => Properties.GetOrDefault(name);
        set => Properties[name] = value;
    }

    /// <summary>
    /// Can be used to get/set custom properties for this feature.
    /// </summary>
    public Dictionary<string, object?> Properties { get; } = [];

    public FeatureDefinition(
        string name,
        string? defaultValue = null,
        LocalizableString? displayName = null,
        LocalizableString? description = null,
        //IStringValueType? valueType = null,
        bool isVisibleToClients = true,
        bool isAvailableToHost = true)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        DefaultValue = defaultValue;
        DisplayName = displayName ?? name;
        Description = description;
        //ValueType = valueType ?? new ToggleStringValueType();
        //IsVisibleToClients = isVisibleToClients;
        //IsAvailableToHost = isAvailableToHost;

        Properties = [];
        //AllowedProviders = new List<string>();
        _children = [];
    }

    /// <summary>
    /// Sets a property in the <see cref="Properties"/> dictionary.
    /// This is a shortcut for nested calls on this object.
    /// </summary>
    public virtual FeatureDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }
}
