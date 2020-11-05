using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Application.Models
{
    public class RoleUpdateInput
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 开启锁定功能
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// 开启TwoFactor功能
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }
    }
}