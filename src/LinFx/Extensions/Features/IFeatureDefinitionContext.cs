using Microsoft.Extensions.Localization;

namespace LinFx.Extensions.Features;

/// <summary>
/// 上下文
/// </summary>
public interface IFeatureDefinitionContext
{
    FeatureGroupDefinition AddGroup(string name, LocalizedString? displayName = null);

    FeatureGroupDefinition? GetGroupOrNull(string name);

    void RemoveGroup(string name);
}
