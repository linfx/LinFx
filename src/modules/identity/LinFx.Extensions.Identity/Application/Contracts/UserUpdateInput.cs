using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Application.Models
{
    public class UserUpdateInput
    {
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
        /// 手机
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "手机")]
        public virtual string PhoneNumber { get; set; }
    }
}