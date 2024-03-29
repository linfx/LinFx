namespace LinFx.Extensions.Features;

/// <summary>
/// 特征定义提供者
/// </summary>
public interface IFeatureDefinitionProvider
{
    /// <summary>
    /// 定义
    /// </summary>
    /// <param name="context"></param>
    void Define(IFeatureDefinitionContext context);
}
