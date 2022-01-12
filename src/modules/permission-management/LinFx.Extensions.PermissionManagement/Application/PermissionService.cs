﻿using LinFx.Application.Services;
using LinFx.Extensions.Authorization.Permissions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Extensions.PermissionManagement;

public class PermissionService : ApplicationService, IPermissionService
{
    protected PermissionManagementOptions Options { get; }
    protected IPermissionManager PermissionManager { get; }
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

    public PermissionService(
        IServiceProvider serviceProvider,
        IPermissionManager permissionManager,
        IPermissionDefinitionManager permissionDefinitionManager,
        IOptions<PermissionManagementOptions> options) : base(serviceProvider)
    {
        Options = options.Value;
        PermissionManager = permissionManager;
        PermissionDefinitionManager = permissionDefinitionManager;
    }

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
}
