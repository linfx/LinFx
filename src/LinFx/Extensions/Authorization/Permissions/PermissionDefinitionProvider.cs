using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 权限定义提供者
/// </summary>
public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider, ITransientDependency
{
    public IStringLocalizer Localizer { get; }

    public PermissionDefinitionProvider() { }

    public PermissionDefinitionProvider(IStringLocalizer localizer)
    {
        Localizer = localizer;
    }

    /// <summary>
    /// 定义
    /// </summary>
    /// <param name="context"></param>
    public abstract void Define(IPermissionDefinitionContext context);

    protected virtual LocalizedString L(string name)
    {
        if (Localizer == null)
            return new LocalizedString(name, name);

        return Localizer[name];
    }
}
