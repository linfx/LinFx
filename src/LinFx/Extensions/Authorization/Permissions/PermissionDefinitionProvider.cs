using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 权限定义提供者
/// </summary>
public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider, ITransientDependency
{
    protected IStringLocalizer _localizer;

    protected PermissionDefinitionProvider(IStringLocalizer localizer)
    {
        _localizer = localizer;
    }

    /// <summary>
    /// 定义
    /// </summary>
    /// <param name="context"></param>
    public abstract void Define(IPermissionDefinitionContext context);

    protected virtual LocalizedString L(string name)
    {
        return _localizer[name];
    }
}
