using LinFx.Security.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace LinFx.Security.Authorization
{
    /// <summary>
    /// Provides programmatic configuration used by <see cref="IAuthorizationService"/> and <see cref="IAuthorizationPolicyProvider"/>.
    /// </summary>
    public class AuthorizationOptions : Microsoft.AspNetCore.Authorization.AuthorizationOptions
    {
        /// <summary>
        /// Gets or sets the <see cref="PermissionOptions"/> for the authorization  system.
        /// </summary>
        /// <value>
        /// The <see cref="PermissionOptions"/> for the authorization system.
        /// </value>
        public PermissionOptions Permissions { get; set; } = new PermissionOptions();
    }
}
