using LinFx.Extensions.FeatureManagement.Application;
using LinFx.Extensions.FeatureManagement.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LinFx.Extensions.FeatureManagement.HttpApi;

/// <summary>
/// 特征管理
/// </summary>
/// <param name="featureAppService"></param>
[ApiController]
[Route("api/feature-management/[controller]")]
public class FeatureController(FeatureService featureAppService) : ControllerBase
{
    protected FeatureService FeatureAppService { get; } = featureAppService;

    /// <summary>
    /// 获取特征
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("/api/feature-management/features")]
    public virtual ValueTask<GetFeatureListResultDto> GetAsync(string providerName, string providerKey) => FeatureAppService.GetAsync(providerName, providerKey);

    /// <summary>
    /// 更新特征
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public virtual Task UpdateAsync(string providerName, string providerKey, UpdateFeaturesDto input) => FeatureAppService.UpdateAsync(providerName, providerKey, input);

    /// <summary>
    /// 删除特征
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    [HttpDelete]
    public virtual Task DeleteAsync(string providerName, string providerKey) => FeatureAppService.DeleteAsync(providerName, providerKey);
}
