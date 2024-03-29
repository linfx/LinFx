using LinFx.Extensions.DependencyInjection;
using System.Collections.Immutable;

namespace LinFx.Extensions.Features;

public class FeatureDefinitionManager(IStaticFeatureDefinitionStore staticStore, IDynamicFeatureDefinitionStore dynamicStore) : IFeatureDefinitionManager, ISingletonDependency
{
    protected IStaticFeatureDefinitionStore StaticStore = staticStore;
    protected IDynamicFeatureDefinitionStore DynamicStore = dynamicStore;

    public virtual async Task<FeatureDefinition> GetAsync(string name)
    {
        var item = await GetOrNullAsync(name);
        if (item == null)
            throw new LinFxException("Undefined feature: " + name);

        return item;
    }

    public virtual async Task<FeatureDefinition?> GetOrNullAsync(string name) => await StaticStore.GetOrNullAsync(name) ?? await DynamicStore.GetOrNullAsync(name);

    public virtual async Task<IReadOnlyList<FeatureDefinition>> GetAllAsync()
    {
        var staticFeatures = await StaticStore.GetFeaturesAsync();
        var staticFeatureNames = staticFeatures
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var dynamicFeatures = await DynamicStore.GetFeaturesAsync();

        /* We prefer static features over dynamics */
        return staticFeatures.Concat(dynamicFeatures.Where(d => !staticFeatureNames.Contains(d.Name))).ToImmutableList();
    }

    public virtual async Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync()
    {
        var staticGroups = await StaticStore.GetGroupsAsync();
        var staticGroupNames = staticGroups
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var dynamicGroups = await DynamicStore.GetGroupsAsync();

        /* We prefer static groups over dynamics */
        return staticGroups.Concat(dynamicGroups.Where(d => !staticGroupNames.Contains(d.Name))).ToImmutableList();
    }
}
