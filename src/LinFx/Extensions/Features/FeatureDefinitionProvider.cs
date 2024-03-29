using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace LinFx.Extensions.Features;

/// <summary>
/// 特征定义提供者
/// </summary>
public abstract class FeatureDefinitionProvider : IFeatureDefinitionProvider, ITransientDependency
{
    [Autowired]
    public ILazyServiceProvider LazyServiceProvider { get; set; }

    //public IStringLocalizer Localizer => LazyServiceProvider.LazyGetRequiredService<IStringLocalizer>();
    public IStringLocalizer Localizer {  get; set; }

    public FeatureDefinitionProvider() { }

    public FeatureDefinitionProvider(IServiceProvider serviceProvider)
    {
        LazyServiceProvider = serviceProvider.GetRequiredService<ILazyServiceProvider>();
    }

    public FeatureDefinitionProvider(IStringLocalizer localizer)
    {
        Localizer = localizer;
    }

    public abstract void Define(IFeatureDefinitionContext context);

    /// <summary>
    /// 多语言
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected virtual LocalizedString L(string name)
    {
        if (Localizer == null)
            return new LocalizedString(name, name);

        return Localizer[name];
    }
}
