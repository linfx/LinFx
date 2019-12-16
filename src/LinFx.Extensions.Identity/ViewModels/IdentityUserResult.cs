using System;

namespace LinFx.Module.Identity.ViewModels
{
    public class IdentityUserResult
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        public virtual string TenantId { set; get; }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// 手机认证
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 最后锁定时间
        /// </summary>
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// 开启TwoFactor功能
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// 登录错误次数
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// 开启锁定功能
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 最后修改者
        /// </summary>
        public string LastModifierId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTimeOffset? LastModificationTime { get; set; }
    }
}
