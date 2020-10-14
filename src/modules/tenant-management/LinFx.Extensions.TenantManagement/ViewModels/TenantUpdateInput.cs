using System.ComponentModel.DataAnnotations;

namespace LinFx.Module.TenantManagement.ViewModels
{
    public class TenantUpdateInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(200)]
        public virtual string Name { get; set; }
    }
}