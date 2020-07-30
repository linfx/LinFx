using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.Permissions
{
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider, IAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
            _options = options.Value;
        }

        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = _options.GetPolicy(policyName);
            if (policy == null)
            {
                var claimValues = policyName.Split(new char[] { '.' }, StringSplitOptions.None);
                if (claimValues.Length == 1)
                {
                    _options.AddPolicy(policyName, builder => builder.AddRequirements(new ClaimsAuthorizationRequirement(claimValues[0], null)));
                }
                else
                {
                    _options.AddPolicy(policyName, builder => builder.AddRequirements(new ClaimsAuthorizationRequirement($"{claimValues[0]}.{claimValues[1]}", new string[] { policyName })));
                }
            }
            return Task.FromResult(_options.GetPolicy(policyName));
        }
    }
}
