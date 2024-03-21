using Microsoft.Extensions.Localization;

namespace LinFx.Extensions.Features;

/// <summary>
/// 上下文
/// </summary>
public class FeatureDefinitionContext : IFeatureDefinitionContext
{
    internal Dictionary<string, FeatureGroupDefinition> Groups { get; }

    public FeatureDefinitionContext()
    {
        Groups = [];
    }

    public FeatureGroupDefinition AddGroup(string name, LocalizedString? displayName = null)
    {
        Check.NotNull(name, nameof(name));

        if (Groups.ContainsKey(name))
        {
            throw new LinFxException($"There is already an existing feature group with name: {name}");
        }

        return Groups[name] = new FeatureGroupDefinition(name, displayName);
    }

    public FeatureGroupDefinition? GetGroupOrNull(string name)
    {
        Check.NotNull(name, nameof(name));

        if (!Groups.ContainsKey(name))
        {
            return null;
        }

        return Groups[name];
    }

    public void RemoveGroup(string name)
    {
        Check.NotNull(name, nameof(name));

        if (!Groups.ContainsKey(name))
        {
            throw new LinFxException($"Undefined feature group: '{name}'.");
        }

        Groups.Remove(name);
    }
}
