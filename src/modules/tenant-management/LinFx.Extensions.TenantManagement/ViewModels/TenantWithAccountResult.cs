namespace LinFx.Module.TenantManagement.ViewModels
{
    public class TenantWithAccountResult : TenantResult
    {
        /// <summary>
        /// 默认租户管理员
        /// </summary>
        public AccountModel Account { get; set; }

        public class AccountModel
        {
            /// <summary>
            /// 账号
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// 角色
            /// </summary>
            public string[] Role { get; set; }
        }
    }
}
