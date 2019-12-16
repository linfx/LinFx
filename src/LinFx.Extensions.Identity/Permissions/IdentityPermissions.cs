namespace LinFx.Extensions.Identity.Permissions
{
    public static class IdentityPermissions
    {
        /// <summary>
        /// 权限组名
        /// </summary>
        public const string GroupName = "Identity";

        /// <summary>
        /// 用户权限项
        /// </summary>
        public static class Users
        {
            public const string Default = GroupName + ".Users";
            public const string Index = Default + ".Index";
            public const string Delete = Default + ".Delete";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Details = Default + ".Details";
        }

        /// <summary>
        /// 角色权限项
        /// </summary>
        public static class Roles
        {
            public const string Default = GroupName + ".Roles";
            public const string Index = Default + ".Index";
            public const string Delete = Default + ".Delete";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Details = Default + ".Details";
            public const string Permissions = Default + ".Permissions";
        }
    }
}