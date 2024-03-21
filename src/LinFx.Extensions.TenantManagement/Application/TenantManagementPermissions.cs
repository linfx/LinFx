namespace LinFx.Extensions.TenantManagement;

/// <summary>
/// 租户管理权限
/// </summary>
public static class TenantManagementPermissions
{
    public const string GroupName = "TenantManagement";

    public static class Tenants
    {
        public const string Default = GroupName + ".Tenants";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
}
