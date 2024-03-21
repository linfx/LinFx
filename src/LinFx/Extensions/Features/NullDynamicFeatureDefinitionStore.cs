using LinFx.Extensions.DependencyInjection;
using System.Collections.Immutable;

namespace LinFx.Extensions.Features;

public class NullDynamicFeatureDefinitionStore : IDynamicFeatureDefinitionStore, ISingletonDependency
{
    private static readonly Task<FeatureDefinition?> CachedFeatureResult = Task.FromResult((FeatureDefinition?)null);

    private static readonly Task<IReadOnlyList<FeatureDefinition>> CachedFeaturesResult =
        Task.FromResult((IReadOnlyList<FeatureDefinition>)Array.Empty<FeatureDefinition>().ToImmutableList());

    private static readonly Task<IReadOnlyList<FeatureGroupDefinition>> CachedGroupsResult =
        Task.FromResult((IReadOnlyList<FeatureGroupDefinition>)Array.Empty<FeatureGroupDefinition>().ToImmutableList());

    public Task<FeatureDefinition?> GetOrNullAsync(string name) => CachedFeatureResult;

    public Task<IReadOnlyList<FeatureDefinition>> GetFeaturesAsync() => CachedFeaturesResult;

    public Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync() => CachedGroupsResult;
}
