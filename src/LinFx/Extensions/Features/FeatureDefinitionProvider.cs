using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Features;

/// <summary>
/// 特征定义提供者
/// </summary>
public abstract class FeatureDefinitionProvider : IFeatureDefinitionProvider, ITransientDependency
{
    [NotNull]
    [Autowired]
    public ILazyServiceProvider? LazyServiceProvider { get; set; }

    public FeatureDefinitionProvider() { }

    public FeatureDefinitionProvider(IServiceProvider serviceProvider) => LazyServiceProvider = serviceProvider.GetRequiredService<ILazyServiceProvider>();

    public abstract void Define(IFeatureDefinitionContext context);

    protected virtual IStringLocalizer L
    {
        get
        {
            if (_localizer == null)
            {
                _localizer = LazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>().Create(GetType());
            }
            return _localizer;
        }
    }
    private IStringLocalizer? _localizer;
}
