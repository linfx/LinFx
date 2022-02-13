using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 授权策略提供者
/// which provides a <see cref="AuthorizationPolicy"/> for a particular name.
/// </summary>
public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider, IAuthorizationPolicyProvider, ITransientDependency
{
    private readonly AuthorizationOptions _options;
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;

    public AuthorizationPolicyProvider(
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
            // 通过 Builder 构建一个策略。
            var policyBuilder = new AuthorizationPolicyBuilder(Array.Empty<string>());

            // 创建一个 PermissionRequirement 对象添加到限定条件组中。
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
