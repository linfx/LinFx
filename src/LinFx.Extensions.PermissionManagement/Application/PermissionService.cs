using LinFx.Application.Services;
using LinFx.Extensions.Authorization;
using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.Caching;
using LinFx.Extensions.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.PermissionManagement;

/// <summary>
/// 权限服务
/// </summary>
public class PermissionService : ApplicationService
{
    protected PermissionManagementOptions Options { get; }

    protected PermissionManagementDbContext Db { get; }

    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

    //protected ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; }

    protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }

    private readonly Lazy<List<IPermissionManagementProvider>> _lazyProviders;

    protected IReadOnlyList<IPermissionManagementProvider> ManagementProviders => _lazyProviders.Value;


    public PermissionService(
        IServiceProvider serviceProvider,
        PermissionManagementDbContext db,
        IPermissionDefinitionManager permissionDefinitionManager,
        IDistributedCache<PermissionGrantCacheItem> cache,
        IOptions<PermissionManagementOptions> options)
    {
        Options = options.Value;
        Db = db;
        PermissionDefinitionManager = permissionDefinitionManager;

        //CurrentTenant = currentTenant;
        Cache = cache;
        PermissionDefinitionManager = permissionDefinitionManager;
        _lazyProviders = new Lazy<List<IPermissionManagementProvider>>(() => Options.ManagementProviders.Select(c => (IPermissionManagementProvider)serviceProvider.GetRequiredService(c)).ToList(), true);
    }

    /// <summary>
    /// 获取权限
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public virtual async ValueTask<PermissionListResultDto> GetAsync(string providerName, string providerKey)
    {
        if (string.IsNullOrEmpty(providerName))
            throw new ArgumentException($"“{nameof(providerName)}”不能为 null 或空。", nameof(providerName));

        if (string.IsNullOrWhiteSpace(providerKey))
            throw new ArgumentException($"“{nameof(providerKey)}”不能为 null 或空白。", nameof(providerKey));

        await CheckProviderPolicy(providerName);

        var result = new PermissionListResultDto
        {
            EntityDisplayName = providerKey,
        };

        //var multiTenancySide = CurrentTenant.GetMultiTenancySide();

        foreach (var group in PermissionDefinitionManager.GetGroups())
        {
            var groupDto = new PermissionGroupDto
            {
                Name = group.Name,
                DisplayName = group.DisplayName,
            };

            var neededCheckPermissions = new List<PermissionDefinition>();

            foreach (var permission in group.GetPermissionsWithChildren())
            //.Where(x => x.IsEnabled)
            //.Where(x => !x.Providers.Any() || x.Providers.Contains(providerName)))
            //.Where(x => x.MultiTenancySide.HasFlag(multiTenancySide))
            {
                //if (await SimpleStateCheckerManager.IsEnabledAsync(permission))
                //    neededCheckPermissions.Add(permission);
                neededCheckPermissions.Add(permission);
            }

            if (neededCheckPermissions.Count == 0)
                continue;

            var grantInfoDtos = neededCheckPermissions.Select(x => new PermissionGrantInfoDto
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                ParentName = x.Parent?.Name!,
                AllowedProviders = x.Providers,
            }).ToList();

            var multipleGrantInfo = await GetAsync(neededCheckPermissions.Select(x => x.Name).ToArray(), providerName, providerKey);

            foreach (var grantInfo in multipleGrantInfo.Result)
            {
                var grantInfoDto = grantInfoDtos.First(x => x.Name == grantInfo.Name);
                grantInfoDto.IsGranted = grantInfo.IsGranted;

                foreach (var provider in grantInfo.Providers)
                {
                    grantInfoDto.GrantedProviders.Add(new ProviderInfoDto
                    {
                        ProviderName = provider.Name,
                        ProviderKey = provider.Key,
                    });
                }

                groupDto.Permissions.Add(grantInfoDto);
            }

            if (groupDto.Permissions.Count > 0)
            {
                result.Groups.Add(groupDto);
            }
        }

        return result;
    }

    /// <summary>
    /// 更新权限
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
    {
        await CheckProviderPolicy(providerName);

        foreach (var permissionDto in input.Permissions)
        {
            await SetAsync(permissionDto.Name, providerName, providerKey, permissionDto.IsGranted);
        }
    }

    protected virtual async Task CheckProviderPolicy(string providerName)
    {
        var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
        if (string.IsNullOrEmpty(policyName))
            throw new Exception($"No policy defined to get/set permissions for the provider '{providerName}'. Use {nameof(PermissionManagementOptions)} to map the policy.");

        await AuthorizationService.CheckAsync(policyName);
    }

    internal async ValueTask<PermissionGrant[]> GetListAsync(string[] names, string providerName, string providerKey) => await Db.PermissionGrants.Where(p => names.Contains(p.Name) && p.ProviderName == providerName && p.ProviderKey == providerKey).ToArrayAsync();

    internal async ValueTask<PermissionGrant?> FindAsync(string name, string providerName, string providerKey) => await Db.PermissionGrants.FirstOrDefaultAsync(p => p.Name == name && p.ProviderName == providerName && p.ProviderKey == providerKey);

    internal async Task InsertAsync(PermissionGrant permissionGrant)
    {
        Db.PermissionGrants.Add(permissionGrant);
        await Db.SaveChangesAsync();
    }

    internal Task DeleteAsync(string providerName, string providerKey)
    {
        throw new NotImplementedException();
    }

    internal Task DeleteAsync(string name, string providerName, string providerKey)
    {
        throw new NotImplementedException();
    }

    internal Task<PermissionGrant> UpdateAsync(PermissionGrant permissionGrant)
    {
        throw new NotImplementedException();
    }

    protected virtual async ValueTask<MultiplePermissionWithGrantedProviders> GetInternalAsync(PermissionDefinition[] permissions, string providerName, string providerKey)
    {
        var permissionNames = permissions.Select(x => x.Name).ToArray();
        var multiplePermissionWithGrantedProviders = new MultiplePermissionWithGrantedProviders(permissionNames);

        var neededCheckPermissions = new List<PermissionDefinition>();

        foreach (var permission in permissions
                                    .Where(x => x.IsEnabled)
                                    //.Where(x => x.MultiTenancySide.HasFlag(CurrentTenant.GetMultiTenancySide()))
                                    .Where(x => !x.Providers.Any() || x.Providers.Contains(providerName)))
        {
            //if (await SimpleStateCheckerManager.IsEnabledAsync(permission))
            //    neededCheckPermissions.Add(permission);
            neededCheckPermissions.Add(permission);
        }

        if (!neededCheckPermissions.Any())
            return multiplePermissionWithGrantedProviders;

        foreach (var provider in ManagementProviders)
        {
            permissionNames = neededCheckPermissions.Select(x => x.Name).ToArray();
            var multiplePermissionValueProviderGrantInfo = await provider.CheckAsync(permissionNames, providerName, providerKey);

            foreach (var providerResultDict in multiplePermissionValueProviderGrantInfo.Result)
            {
                if (providerResultDict.Value.IsGranted)
                {
                    var permissionWithGrantedProvider = multiplePermissionWithGrantedProviders.Result.First(x => x.Name == providerResultDict.Key);
                    permissionWithGrantedProvider.IsGranted = true;
                    permissionWithGrantedProvider.Providers.Add(new PermissionValueProviderInfo(provider.Name, providerResultDict.Value.ProviderKey));
                }
            }
        }

        return multiplePermissionWithGrantedProviders;
    }


    public virtual ValueTask<PermissionWithGrantedProviders> GetAsync(string permissionName, string providerName, string providerKey) => GetInternalAsync(PermissionDefinitionManager.Get(permissionName), providerName, providerKey);

    public virtual async ValueTask<MultiplePermissionWithGrantedProviders> GetAsync(string[] permissionNames, string providerName, string providerKey)
    {
        var permissionDefinitions = permissionNames.Select(x => PermissionDefinitionManager.Get(x)).ToArray();
        return await GetInternalAsync(permissionDefinitions, providerName, providerKey);
    }

    public virtual async ValueTask<List<PermissionWithGrantedProviders>> GetAllAsync(string providerName, string providerKey)
    {
        var permissionDefinitions = PermissionDefinitionManager.GetPermissions().ToArray();
        var multiplePermissionWithGrantedProviders = await GetInternalAsync(permissionDefinitions, providerName, providerKey);
        return multiplePermissionWithGrantedProviders.Result;

    }

    /// <summary>
    /// 设置权限
    /// </summary>
    /// <param name="permissionName"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="isGranted"></param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    public virtual async Task SetAsync(string permissionName, string providerName, string providerKey, bool isGranted)
    {
        var permission = PermissionDefinitionManager.Get(permissionName);
        if (!permission.IsEnabled)
            throw new BusinessException($"The permission named '{permission.Name}' is disabled!");

        if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
            throw new BusinessException($"The permission named '{permission.Name}' has not compatible with the provider named '{providerName}'");

        //if (!permission.MultiTenancySide.HasFlag(CurrentTenant.GetMultiTenancySide()))
        //{
        //    //TODO: BusinessException
        //    throw new ApplicationException($"The permission named '{permission.Name}' has multitenancy side '{permission.MultiTenancySide}' which is not compatible with the current multitenancy side '{CurrentTenant.GetMultiTenancySide()}'");
        //}

        var currentGrantInfo = await GetInternalAsync(permission, providerName, providerKey);
        if (currentGrantInfo.IsGranted == isGranted)
            return;

        var provider = ManagementProviders.FirstOrDefault(m => m.Name == providerName);
        if (provider == null)
        {
            throw new BusinessException("Unknown permission management provider: " + providerName);
        }

        await provider.SetAsync(permissionName, providerKey, isGranted);
    }

    public virtual async ValueTask<PermissionGrant> UpdateProviderKeyAsync(PermissionGrant permissionGrant, string providerKey)
    {
        using (CurrentTenant.Change(permissionGrant.TenantId))
        {
            //Invalidating the cache for the old key
            await Cache.RemoveAsync(PermissionGrantCacheItem.CalculateCacheKey(permissionGrant.Name, permissionGrant.ProviderName, permissionGrant.ProviderKey));
        }

        permissionGrant.ProviderKey = providerKey;
        return await UpdateAsync(permissionGrant);
    }

    protected virtual async ValueTask<PermissionWithGrantedProviders> GetInternalAsync(PermissionDefinition permission, string providerName, string providerKey) => (await GetInternalAsync([permission], providerName, providerKey)).Result.First();
}
