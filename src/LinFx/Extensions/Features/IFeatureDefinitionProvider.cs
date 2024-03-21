namespace LinFx.Extensions.Features;

/// <summary>
/// 特征定义提供者
/// </summary>
public interface IFeatureDefinitionProvider
{
    void Define(IFeatureDefinitionContext context);
}
