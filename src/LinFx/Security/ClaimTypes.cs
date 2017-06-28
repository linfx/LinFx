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
        /// Default: <see cref="System.Security.Claims.ClaimTypes.Name"/>
        /// </summary>
        public static string UserName { get; set; } = System.Security.Claims.ClaimTypes.Name;
        /// <summary>
        /// UserId.
        /// Default: <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>
        /// </summary>
        public static string UserId { get; set; } = System.Security.Claims.ClaimTypes.NameIdentifier;
        /// <summary>
        /// UserId.
        /// Default: <see cref="ClaimTypes.Role"/>
        /// </summary>
        public static string Role { get; set; } = System.Security.Claims.ClaimTypes.Role;
    }
}
