using LinFx.Reflection;

namespace LinFx.Extensions.FeatureManagement.Application;

public class FeatureManagementPermissions
{
    public const string GroupName = "FeatureManagement";

    public const string ManageHostFeatures = GroupName + ".ManageHostFeatures";

    public static string[] GetAll() => ReflectionHelper.GetPublicConstantsRecursively(typeof(FeatureManagementPermissions));
}
