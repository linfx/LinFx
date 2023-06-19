using LinFx.Application.Services;
using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.PermissionManagement.Application;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.PermissionManagement;

/// <summary>
/// 权限服务
/// </summary>
public class PermissionService : ApplicationService
{
    protected PermissionManagementOptions Options { get; }
    protected PermissionManager PermissionManager { get; }
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

    public PermissionService(
        PermissionManager permissionManager,
        IPermissionDefinitionManager permissionDefinitionManager,
        IOptions<PermissionManagementOptions> options)
    {
        Options = options.Value;
        PermissionManager = permissionManager;
        PermissionDefinitionManager = permissionDefinitionManager;
    }

    /// <summary>
    /// 获取权限
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public virtual async Task<PermissionListResultDto> GetAsync(string providerName, string providerKey)
    {
        await CheckProviderPolicy(providerName);

        var result = new PermissionListResultDto
        {
            EntityDisplayName = providerKey,
            Groups = new List<PermissionGroupDto>()
        };

        //var multiTenancySide = CurrentTenant.GetMultiTenancySide();

        foreach (var group in PermissionDefinitionManager.GetGroups())
        {
            var groupDto = new PermissionGroupDto
            {
                Name = group.Name,
                DisplayName = group.DisplayName,
                Permissions = new List<PermissionGrantInfoDto>()
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

            if (!neededCheckPermissions.Any())
                continue;

            var grantInfoDtos = neededCheckPermissions.Select(x => new PermissionGrantInfoDto
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                ParentName = x.Parent?.Name,
                AllowedProviders = x.Providers,
                GrantedProviders = new List<ProviderInfoDto>()
            }).ToList();

            var multipleGrantInfo = await PermissionManager.GetAsync(neededCheckPermissions.Select(x => x.Name).ToArray(), providerName, providerKey);

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

            if (groupDto.Permissions.Any())
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
            await PermissionManager.SetAsync(permissionDto.Name, providerName, providerKey, permissionDto.IsGranted);
        }
    }

    protected virtual async Task CheckProviderPolicy(string providerName)
    {
        var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
        //if (policyName.IsNullOrEmpty())
        //    throw new Exception($"No policy defined to get/set permissions for the provider '{providerName}'. Use {nameof(PermissionManagementOptions)} to map the policy.");

        //await AuthorizationService.CheckAsync(policyName);
        await Task.CompletedTask;
    }

    internal Task GetListAsync(string providerName, string providerKey)
    {
        throw new NotImplementedException();
    }

    internal ValueTask<PermissionGrant[]> GetListAsync(string[] names, string providerName, string providerKey)
    {
        throw new NotImplementedException();
    }

    internal ValueTask<PermissionGrant> FindAsync(string name1, string name2, string providerKey)
    {
        throw new NotImplementedException();
    }

    internal Task InsertAsync(PermissionGrant permissionGrant)
    {
        throw new NotImplementedException();
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
}
