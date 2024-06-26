﻿using System.Security.Claims;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 权限检查器
/// </summary>
public interface IPermissionChecker
{
    Task<PermissionGrantInfo> IsGrantedAsync(string name);

    Task<PermissionGrantInfo> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name);
}
