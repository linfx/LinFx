﻿using System.Security.Claims;
using System.Threading.Tasks;

namespace LinFx.Extensions.Authorization.Permissions
{
    /// <summary>
    /// Always allows for any permission.
    /// Use IServiceCollection.AddAlwaysAllowAuthorization() to replace
    /// IPermissionChecker with this class. This is useful for tests.
    /// </summary>
    public class AlwaysAllowPermissionChecker : IPermissionChecker
    {
        public Task<PermissionGrantInfo> IsGrantedAsync(string name)
        {
            return Task.FromResult(new PermissionGrantInfo(name, true, "AlwaysAllow"));
        }

        public Task<PermissionGrantInfo> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            return Task.FromResult(new PermissionGrantInfo(name, true, "AlwaysAllow"));
        }
    }
}
