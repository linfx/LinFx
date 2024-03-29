using JetBrains.Annotations;

namespace LinFx.Extensions.Features;

/// <summary>
/// 特征值提供者
/// </summary>
public interface IFeatureValueProvider
{
    string Name { get; }

    Task<string?> GetOrNullAsync([NotNull] FeatureDefinition feature);
}
