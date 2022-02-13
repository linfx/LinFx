namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 权限定义提供者
/// </summary>
public interface IPermissionDefinitionProvider
{
    /// <summary>
    /// 定义
    /// </summary>
    /// <param name="context"></param>
    void Define(IPermissionDefinitionContext context);
}
