using LinFx.Extensions.DependencyInjection;

namespace LinFx.Extensions.Features;

/// <summary>
/// 特征定义提供者
/// </summary>
public abstract class FeatureDefinitionProvider : IFeatureDefinitionProvider, ITransientDependency
{
    public abstract void Define(IFeatureDefinitionContext context);
}
