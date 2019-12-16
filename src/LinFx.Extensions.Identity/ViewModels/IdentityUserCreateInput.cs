using System.ComponentModel.DataAnnotations;

namespace LinFx.Module.Identity.ViewModels
{
    public class IdentityUserCreateInput
    {
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
        public virtual string Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(100)]
        [Display(Name = "姓名")]
        public virtual string Name { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(15, ErrorMessage = "{0}长度为{2}-{1}个字符", MinimumLength = 2)]
        [Display(Name = "账号")]
        public virtual string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(50, ErrorMessage = "{0}长度为{2}-{1}个字符", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public virtual string Password { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Phone]
        [Display(Name = "手机")]
        public virtual string PhoneNumber { get; set; }
    }
}
