namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 权限值提供者
/// </summary>
public abstract class PermissionValueProvider : IPermissionValueProvider
{
    public abstract string Name { get; }

    protected IPermissionStore PermissionStore { get; }

    protected PermissionValueProvider(IPermissionStore permissionStore)
    {
        PermissionStore = permissionStore;
    }

    public abstract Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context);
}
