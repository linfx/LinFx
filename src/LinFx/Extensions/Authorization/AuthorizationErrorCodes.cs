namespace LinFx.Extensions.Authorization;

public static class AuthorizationErrorCodes
{
    public const string GivenPolicyHasNotGranted = "Authorization:010001";

    public const string GivenPolicyHasNotGrantedWithPolicyName = "Authorization:010002";

    public const string GivenPolicyHasNotGrantedForGivenResource = "Authorization:010003";

    public const string GivenRequirementHasNotGrantedForGivenResource = "Authorization:010004";

    public const string GivenRequirementsHasNotGrantedForGivenResource = "Authorization:010005";
}