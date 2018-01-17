namespace LinFx.Security
{
    /// <summary>
    /// Used to get specific claim type names.
    /// </summary>
    public static class ClaimTypes
    {
        /// <summary>
        /// TenantId.
        /// </summary>
        public const string TenantId = "http://linsongbin.cnblogs.com/identity/claims/tenantId";
        /// <summary>
        /// UserId.
        /// Default: <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>
        /// </summary>
        public static string Id { get; set; } = System.Security.Claims.ClaimTypes.NameIdentifier;
        /// <summary>
        /// UserId.
        /// Default: <see cref="System.Security.Claims.ClaimTypes.Name"/>
        /// </summary>
        public static string Name { get; set; } = System.Security.Claims.ClaimTypes.Name;
        /// <summary>
        /// UserId.
        /// Default: <see cref="ClaimTypes.Role"/>
        /// </summary>
        public static string Role { get; set; } = System.Security.Claims.ClaimTypes.Role;
        /// <summary>
        /// client_id
        /// </summary>
        public const string Client_Id = "https://github.com/linfx/identity/claims/client_id";
        /// <summary>
        /// client_secret
        /// </summary>
        public const string Client_Secret = "https://github.com/linfx/identity/claims/client_secret";
    }
}
