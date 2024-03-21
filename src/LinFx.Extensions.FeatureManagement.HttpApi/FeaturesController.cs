using LinFx.Extensions.FeatureManagement.Application;
using LinFx.Extensions.FeatureManagement.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LinFx.Extensions.FeatureManagement.HttpApi;

[ApiController]
[Route("api/feature-management/features")]
public class FeaturesController(FeatureService featureAppService) : ControllerBase
{
    protected FeatureService FeatureAppService { get; } = featureAppService;

    [HttpGet]
    public virtual Task<GetFeatureListResultDto> GetAsync(string providerName, string providerKey) => FeatureAppService.GetAsync(providerName, providerKey);

    [HttpPut]
    public virtual Task UpdateAsync(string providerName, string providerKey, UpdateFeaturesDto input) => FeatureAppService.UpdateAsync(providerName, providerKey, input);

    [HttpDelete]
    public virtual Task DeleteAsync(string providerName, string providerKey) => FeatureAppService.DeleteAsync(providerName, providerKey);
}
