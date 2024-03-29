using JetBrains.Annotations;
using LinFx.Application.Services;
using LinFx.Extensions.FeatureManagement.Application.Dtos;
using LinFx.Extensions.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.FeatureManagement.Application;

[Authorize]
public class FeatureService(
    IFeatureManager featureManager,
    IFeatureDefinitionManager featureDefinitionManager,
    IOptions<FeatureManagementOptions> options) : ApplicationService
{
    protected FeatureManagementOptions Options { get; } = options.Value;

    protected IFeatureManager FeatureManager { get; } = featureManager;

    protected IFeatureDefinitionManager FeatureDefinitionManager { get; } = featureDefinitionManager;

    public virtual async Task<GetFeatureListResultDto> GetAsync([NotNull] string providerName, string providerKey)
    {
        await CheckProviderPolicy(providerName, providerKey);

        var result = new GetFeatureListResultDto();

        foreach (var group in await FeatureDefinitionManager.GetGroupsAsync())
        {
            var groupDto = CreateFeatureGroupDto(group);

            foreach (var featureDefinition in group.GetFeaturesWithChildren())
            {
                var feature = await FeatureManager.GetOrNullWithProviderAsync(featureDefinition.Name, providerName, providerKey);
                groupDto.Features.Add(CreateFeatureDto(feature, featureDefinition));
            }

            SetFeatureDepth(groupDto.Features, providerName, providerKey);

            if (groupDto.Features.Any())
            {
                result.Groups.Add(groupDto);
            }
        }

        return result;
    }

    private static FeatureGroupDto CreateFeatureGroupDto(FeatureGroupDefinition groupDefinition) => new FeatureGroupDto
    {
        Name = groupDefinition.Name,
        DisplayName = groupDefinition.DisplayName,
        Features = new List<FeatureDto>()
    };

    private static FeatureDto CreateFeatureDto(FeatureNameValueWithGrantedProvider featureNameValueWithGrantedProvider, FeatureDefinition featureDefinition) => new FeatureDto
    {
        Name = featureDefinition.Name,
        DisplayName = featureDefinition.DisplayName,
        Description = featureDefinition.Description,
        //ValueType = featureDefinition.ValueType,
        ParentName = featureDefinition.Parent?.Name,
        Value = featureNameValueWithGrantedProvider.Value,
        Provider = new FeatureProviderDto
        {
            Name = featureNameValueWithGrantedProvider.Provider?.Name!,
            Key = featureNameValueWithGrantedProvider.Provider?.Key!
        }
    };

    public virtual async Task UpdateAsync([NotNull] string providerName, string providerKey, UpdateFeaturesDto input)
    {
        await CheckProviderPolicy(providerName, providerKey);

        foreach (var feature in input.Features)
        {
            await FeatureManager.SetAsync(feature.Name, feature.Value, providerName, providerKey);
        }
    }

    protected virtual void SetFeatureDepth(List<FeatureDto> features, string providerName, string providerKey, FeatureDto? parentFeature = null, int depth = 0)
    {
        foreach (var feature in features)
        {
            if (parentFeature == null && feature.ParentName == null || parentFeature != null && parentFeature.Name == feature.ParentName)
            {
                feature.Depth = depth;
                SetFeatureDepth(features, providerName, providerKey, feature, depth + 1);
            }
        }
    }

    protected virtual Task CheckProviderPolicy(string providerName, string providerKey)
    {
        //string policyName;
        //if (providerName == TenantFeatureValueProvider.ProviderName && CurrentTenant.Id == null && providerKey == null)
        //{
        //    policyName = FeatureManagementPermissions.ManageHostFeatures;
        //}
        //else
        //{
        //    policyName = Options.ProviderPolicies.GetOrDefault(providerName);
        //    if (policyName.IsNullOrEmpty())
        //    {
        //        throw new AbpException($"No policy defined to get/set permissions for the provider '{providerName}'. Use {nameof(FeatureManagementOptions)} to map the policy.");
        //    }
        //}

        //await AuthorizationService.CheckAsync(policyName);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync([NotNull] string providerName, string providerKey) => FeatureManager.DeleteAsync(providerName, providerKey);
}
