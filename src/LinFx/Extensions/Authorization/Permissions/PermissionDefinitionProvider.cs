using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 权限定义提供者
/// </summary>
public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider, ITransientDependency
{
    [NotNull]
    [Autowired]
    public ILazyServiceProvider? LazyServiceProvider { get; set; }

    public IStringLocalizer Localizer => LazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>().Create(GetType());

    public PermissionDefinitionProvider() { }

    public PermissionDefinitionProvider(IServiceProvider serviceProvider) => LazyServiceProvider = serviceProvider.GetRequiredService<ILazyServiceProvider>();

    public abstract void Define(IPermissionDefinitionContext context);

    /// <summary>
    /// 本地化
    /// </summary>
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
