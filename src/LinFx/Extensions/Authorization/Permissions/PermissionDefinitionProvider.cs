using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 权限定义提供者
/// </summary>
public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider, ITransientDependency
{
    [Autowired]
    public ILazyServiceProvider LazyServiceProvider { get; set; }

    public IStringLocalizer Localizer => LazyServiceProvider.LazyGetRequiredService<IStringLocalizer>();

    public PermissionDefinitionProvider() { }

    public PermissionDefinitionProvider(IServiceProvider serviceProvider)
    {
        LazyServiceProvider = serviceProvider.GetRequiredService<ILazyServiceProvider>();
    }

    public abstract void Define(IPermissionDefinitionContext context);

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
