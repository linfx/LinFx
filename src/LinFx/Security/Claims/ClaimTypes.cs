namespace LinFx.Security.Claims;

public static class ClaimTypes
{
    /// <summary>
    /// Default: <see cref="System.Security.Claims.ClaimTypes.Name"/>
    /// </summary>
    public static string UserName { get; set; } = System.Security.Claims.ClaimTypes.Name;

    /// <summary>
    /// Default: <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>
    /// </summary>
    public const string Id = System.Security.Claims.ClaimTypes.NameIdentifier;

    /// <summary>
    /// Default: <see cref="System.Security.Claims.ClaimTypes.Role"/>
    /// </summary>
    public const string Role = System.Security.Claims.ClaimTypes.Role;

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
    public const string PhoneNumber = JwtClaimTypes.PhoneNumber;

    /// <summary>
    /// Default: "phone_number_verified".
    /// </summary>
    public static string PhoneNumberVerified { get; set; } = "phone_number_verified";

    /// <summary>
    /// Default: "tenantid".
    /// </summary>
    public const string TenantId = JwtClaimTypes.TenantId;

    /// <summary>
    /// Default: "client_id".
    /// </summary>
    public const string ClientId = JwtClaimTypes.ClientId;

    /// <summary>
    /// Default: "edition_id".
    /// </summary>
    public const string EditionId = "edition_id";
}
