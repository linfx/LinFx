namespace LinFx.Extensions.Identity.Permissions
{
    public static class IdentityPermissions
    {
        /// <summary>
        /// 权限组名称
        /// </summary>
        public const string GroupName = "Identity";

        /// <summary>
        /// 角色权限项
        /// </summary>
        public static class Roles
        {
            public const string Default = GroupName + ".Roles";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        /// <summary>
        /// 用户权限项
        /// </summary>
        public static class Users
        {
            public const string Default = GroupName + ".Users";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }
    }
}