using LinFx.Domain.Entities;
using System;

namespace LinFx.Authorization.Accounts
{
    public class Account : Entity
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
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string PhoneNumber { get; set; }
        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreateTime { get; set; }
        ///// <summary>
        ///// 更新时间
        ///// </summary>
        //public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Is the <see cref="Email"/> confirmed.
        /// </summary>
        public virtual bool IsEmailConfirmed { get; set; }
        /// <summary>
        /// Is the <see cref="PhoneNumber"/> confirmed.
        /// </summary>
        public virtual bool IsPhoneNumberConfirmed { get; set; }
    }
}
