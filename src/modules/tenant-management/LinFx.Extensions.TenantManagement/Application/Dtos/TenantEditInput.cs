using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.TenantManagement.Application.Dtos
{
    public class TenantEditInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(64)]
        public virtual string Name { get; set; }
    }
}