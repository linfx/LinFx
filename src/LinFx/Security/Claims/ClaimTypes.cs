namespace LinFx.Security.Claims
{
    public static class ClaimTypes
    {
        /// <summary>
        /// Default: <see cref="System.Security.Claims.ClaimTypes.Name"/>
        /// </summary>
        public static string UserName { get; set; } = System.Security.Claims.ClaimTypes.Name;

        /// <summary>
        /// Default: <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>
        /// </summary>
        public static string Id { get; set; } = System.Security.Claims.ClaimTypes.NameIdentifier;

        /// <summary>
        /// Default: <see cref="System.Security.Claims.ClaimTypes.Role"/>
        /// </summary>
        public static string Role { get; set; } = System.Security.Claims.ClaimTypes.Role;

        /// <summary>
        /// Default: <see cref="System.Security.Claims.ClaimTypes.Email"/>
        /// </summary>
        public static string Email { get; set; } = System.Security.Claims.ClaimTypes.Email;

        /// <summary>
        /// Default: "email_verified".
        /// </summary>
        public static string EmailVerified { get; set; } = "email_verified";

        /// <summary>
        /// Default: "phone_number".
        /// </summary>
        public static string PhoneNumber { get; set; } = "phone_number";

        /// <summary>
        /// Default: "phone_number_verified".
        /// </summary>
        public static string PhoneNumberVerified { get; set; } = "phone_number_verified";

        /// <summary>
        /// Default: "tenantid".
        /// </summary>
        public static string TenantId { get; set; } = "tenant_id";

        /// <summary>
        /// Default: "client_id".
        /// </summary>
        public static string ClientId { get; set; } = "client_id";
    }
}
