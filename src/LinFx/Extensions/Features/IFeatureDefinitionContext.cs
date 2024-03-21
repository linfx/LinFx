using Microsoft.CodeAnalysis;

namespace LinFx.Extensions.Features;

/// <summary>
/// 上下文
/// </summary>
public interface IFeatureDefinitionContext
{
    FeatureGroupDefinition AddGroup(string name, LocalizableString? displayName = null);

    FeatureGroupDefinition? GetGroupOrNull(string name);

    void RemoveGroup(string name);
}
