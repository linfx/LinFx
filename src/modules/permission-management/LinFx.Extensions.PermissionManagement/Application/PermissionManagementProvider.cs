using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.Guids;
using LinFx.Extensions.MultiTenancy;
using LinFx.Utils;

namespace LinFx.Extensions.PermissionManagement.Application;

public abstract class PermissionManagementProvider : IPermissionManagementProvider
{
    /// <summary>
    /// ProviderName
    /// </summary>
    public abstract string Name { get; }

    protected PermissionService PermissionService { get; }

    protected IGuidGenerator GuidGenerator { get; }

    protected ICurrentTenant CurrentTenant { get; }

    protected PermissionManagementProvider(
        PermissionService permissionService,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
    {
        PermissionService = permissionService;
        GuidGenerator = guidGenerator;
        CurrentTenant = currentTenant;
    }

    public virtual async Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
    {
        var multiplePermissionValueProviderGrantInfo = await CheckAsync(new[] { name }, providerName, providerKey);
        return multiplePermissionValueProviderGrantInfo.Result.First().Value;
    }

    public virtual async Task<MultiplePermissionValueProviderGrantInfo> CheckAsync(string[] names, string providerName, string providerKey)
    {
        var multiplePermissionValueProviderGrantInfo = new MultiplePermissionValueProviderGrantInfo(names);
        if (providerName != Name)
            return multiplePermissionValueProviderGrantInfo;

        var permissionGrants = await PermissionService.GetListAsync(names, providerName, providerKey);

        foreach (var permissionName in names)
        {
            var isGrant = permissionGrants.Any(x => x.Name == permissionName);
            multiplePermissionValueProviderGrantInfo.Result[permissionName] = new PermissionValueProviderGrantInfo(isGrant, providerKey);
        }

        return multiplePermissionValueProviderGrantInfo;
    }

    public virtual Task SetAsync(string name, string providerKey, bool isGranted) => isGranted ? GrantAsync(name, providerKey) : RevokeAsync(name, providerKey);

    /// <summary>
    /// 授权
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    protected virtual async Task GrantAsync(string name, string providerKey)
    {
        var item = await PermissionService.FindAsync(name, Name, providerKey);
        if (item != null)
            return;

        await PermissionService.InsertAsync(new PermissionGrant(IDUtils.NewId(), name, Name, providerKey, CurrentTenant.Id));
    }

    /// <summary>
    /// 撤销
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    protected virtual async Task RevokeAsync(string name, string providerKey) => await PermissionService.DeleteAsync(name, Name, providerKey);
}