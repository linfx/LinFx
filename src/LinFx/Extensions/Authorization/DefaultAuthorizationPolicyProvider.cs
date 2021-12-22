using LinFx.Extensions.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Authorization
{
    /// <summary>
    /// 授权策略提供者
    /// which provides a <see cref="AuthorizationPolicy"/> for a particular name.
    /// </summary>
    public class DefaultAuthorizationPolicyProvider : Microsoft.AspNetCore.Authorization.DefaultAuthorizationPolicyProvider, IAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        public DefaultAuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options,
            IPermissionDefinitionManager permissionDefinitionManager)
            : base(options)
        {
            _options = options.Value;
            _permissionDefinitionManager = permissionDefinitionManager;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy != null)
                return policy;

            var permission = _permissionDefinitionManager.GetOrNull(policyName);
            if (permission != null)
            {
                //TODO: Optimize & Cache!
                var policyBuilder = new AuthorizationPolicyBuilder(Array.Empty<string>());
                policyBuilder.Requirements.Add(new PermissionRequirement(policyName));
                return policyBuilder.Build();
            }

            return null;
        }

        //public Task<List<string>> GetPoliciesNamesAsync()
        //{
        //    return Task.FromResult(
        //        _options.GetPoliciesNames()
        //            .Union(
        //                _permissionDefinitionManager
        //                    .GetPermissions()
        //                    .Select(p => p.Name)
        //            )
        //            .ToList()
        //    );
        //}
    }
}