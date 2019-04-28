using System;
using System.ComponentModel.DataAnnotations;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.MultiTenancy;

namespace LinFx.Identity
{
    /// <summary>
    /// Represents a user in the identity system
    /// </summary>
    public class IdentityUser : Microsoft.AspNetCore.Identity.IdentityUser, IMultiTenant, IHasCreationTime, IHasModificationTime
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        public Guid? TenantId { set; get; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
    }
}
