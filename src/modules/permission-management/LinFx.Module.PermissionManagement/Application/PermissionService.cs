using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.PermissionManagement.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Extensions.PermissionManagement.Application
{
    public class PermissionService : IPermissionService
    {
        private readonly PermissionManager _permissionManager;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;
        private readonly IAuthorizationService AuthorizationService;
        protected PermissionManagementOptions Options { get; }

        public PermissionService(
            PermissionManager permissionManager,
            IPermissionDefinitionManager permissionDefinitionManager,
            IOptions<PermissionManagementOptions> options)
        {
            _permissionManager = permissionManager;
            _permissionDefinitionManager = permissionDefinitionManager;
            Options = options.Value;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        public async Task<PermissionListResult> GetAsync(string providerName, string providerKey)
        {
            await CheckProviderPolicy(providerName);

            var result = new PermissionListResult
            {
                //EntityDisplayName = providerKey,
                //Groups = new List<PermissionGroupDto>()
            };

            //var multiTenancySide = CurrentTenant.GetMultiTenancySide();

            foreach (var group in _permissionDefinitionManager.GetGroups())
            {
                var groupDto = new PermissionGroupDto
                {
                    Name = group.Name,
                    DisplayName = group.DisplayName,
                    Permissions = new List<PermissionGrantInfoDto>()
                };

                foreach (var permission in group.GetPermissionsWithChildren())
                {
                    if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
                        continue;

                    //if (!permission.MultiTenancySide.HasFlag(multiTenancySide))
                    //{
                    //    continue;
                    //}

                    var grantInfo = await _permissionManager.GetAsync(permission.Name, providerName, providerKey);

                    var grantInfoDto = new PermissionGrantInfoDto
                    {
                        Name = permission.Name,
                        DisplayName = permission.DisplayName,
                        ParentName = permission.Parent?.Name,
                        AllowedProviders = permission.Providers,
                        IsGranted = grantInfo.IsGranted,
                        GrantedProviders = new List<ProviderInfoDto>(),
                    };

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
                    //result.Groups.Add(groupDto);
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
        public async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionDto input)
        {
            await CheckProviderPolicy(providerName);

            //foreach (var permissionDto in input.Permissions)
            //{
            //    await _permissionManager.SetAsync(permissionDto.Name, providerName, providerKey, permissionDto.IsGranted);
            //}
        }

        protected virtual async Task CheckProviderPolicy(string providerName)
        {
            var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
            //if (policyName.IsNullOrEmpty())
            //{
            //    throw new Exception($"No policy defined to get/set permissions for the provider '{policyName}'. Use {nameof(PermissionManagementOptions)} to map the policy.");
            //}

            //await AuthorizationService.CheckAsync(policyName);
        }
    }
}
