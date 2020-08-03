﻿using System.Security.Claims;

namespace LinFx.Security.Users
{
    /// <summary>
    /// 当前用户
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// 是否认证
        /// </summary>
        bool IsAuthenticated { get; }

        string Id { get; }

        string UserName { get; }

        string PhoneNumber { get; }

        bool PhoneNumberVerified { get; }

        string Email { get; }

        bool EmailVerified { get; }

        string TenantId { get; }

        string[] Roles { get; }

        Claim FindClaim(string claimType);

        Claim[] FindClaims(string claimType);

        Claim[] GetAllClaims();

        bool IsInRole(string roleName);
    }
}
