using LinFx.Domain.Models.Auditing;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.TenantManagement
{
    /// <summary>
    /// 租户
    /// </summary>
    public class Tenant : FullAuditedAggregateRoot<string>
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        [Required]
        [StringLength(64)]
        public virtual string Name { get; set; }

        public Tenant() { }

        public Tenant(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
