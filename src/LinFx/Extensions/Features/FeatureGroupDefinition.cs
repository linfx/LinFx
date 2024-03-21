using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace LinFx.Extensions.Features;

/// <summary>
/// 特征组定义
/// </summary>
public class FeatureGroupDefinition
{
    private readonly List<FeatureDefinition> _features;

    /// <summary>
    /// Unique name of the group.
    /// </summary>
    public string Name { get; }

    public Dictionary<string, object?> Properties { get; }

    public LocalizableString DisplayName { get; set; }

    public IReadOnlyList<FeatureDefinition> Features => _features.ToImmutableList();

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

    protected internal FeatureGroupDefinition(string name, LocalizableString? displayName = null)
    {
        Name = name;
        DisplayName = displayName ?? name;

        Properties = [];
        _features = [];
    }

    public virtual FeatureDefinition AddFeature(
        string name,
        string? defaultValue = null,
        LocalizableString? displayName = null,
        LocalizableString? description = null,
        //StringValueType? valueType = null,
        bool isVisibleToClients = true,
        bool isAvailableToHost = true)
    {
        var feature = new FeatureDefinition(
            name,
            defaultValue,
            displayName,
            description,
            //valueType,
            isVisibleToClients,
            isAvailableToHost
        );
        _features.Add(feature);

        return feature;
    }

    //public FeatureDefinition CreateChildFeature(string name,
    //    string? defaultValue = null,
    //    ILocalizableString? displayName = null,
    //    ILocalizableString? description = null,
    //    IStringValueType? valueType = null,
    //    bool isVisibleToClients = true,
    //    bool isAvailableToHost = true)
    //{
    //    return AddFeature(name, defaultValue, displayName, description, valueType, isVisibleToClients, isAvailableToHost);
    //}
    //public virtual List<FeatureDefinition> GetFeaturesWithChildren()
    //{
    //    var features = new List<FeatureDefinition>();

    //    foreach (var feature in _features)
    //    {
    //        AddFeatureToListRecursively(features, feature);
    //    }

    //    return features;
    //}

    public virtual List<FeatureDefinition> GetFeaturesWithChildren()
    {
        var features = new List<FeatureDefinition>();

        foreach (var feature in _features)
        {
            AddFeatureToListRecursively(features, feature);
        }

        return features;
    }

    /// <summary>
    /// Sets a property in the <see cref="Properties"/> dictionary.
    /// This is a shortcut for nested calls on this object.
    /// </summary>
    public virtual FeatureGroupDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    private void AddFeatureToListRecursively(List<FeatureDefinition> features, FeatureDefinition feature)
    {
        features.Add(feature);

        foreach (var child in feature.Children)
        {
            AddFeatureToListRecursively(features, child);
        }
    }

    public override string ToString() => $"[{nameof(FeatureGroupDefinition)} {Name}]";
}
