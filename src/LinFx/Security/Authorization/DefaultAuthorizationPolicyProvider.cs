using LinFx.Security.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace LinFx.Security.Authorization
{
    /// <summary>
    /// The default implementation of a policy provider,
    /// which provides a <see cref="AuthorizationPolicy"/> for a particular name.
    /// </summary>
    public class DefaultAuthorizationPolicyProvider : Microsoft.AspNetCore.Authorization.DefaultAuthorizationPolicyProvider, IAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;
        private Task<AuthorizationPolicy> _cachedDefaultPolicy;
        private Task<AuthorizationPolicy> _cachedRequiredPolicy;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        public DefaultAuthorizationPolicyProvider(
            [NotNull] IOptions<AuthorizationOptions> options,
            [NotNull] IPermissionDefinitionManager permissionDefinitionManager)
            : base(options)
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(permissionDefinitionManager, nameof(permissionDefinitionManager));

            _options = options.Value;
            _permissionDefinitionManager = permissionDefinitionManager;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy != null)
            {
                return policy;
            }

            var permission = _permissionDefinitionManager.GetOrNull(policyName);
            if (permission != null)
            {
                //TODO: Optimize & Cache!
                var policyBuilder = new AuthorizationPolicyBuilder(Array.Empty<string>());
                policyBuilder.Requirements.Add(new PermissionAuthorizationRequirement(policyName));
                return policyBuilder.Build();
            }

            return null;
        }
    }
}