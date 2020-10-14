using LinFx.Domain.Models;
using LinFx.Extensions.MultiTenancy;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.PermissionManagement
{
    /// <summary>
    /// 权限授权
    /// </summary>
    public class PermissionGrant : Entity<long>, IMultiTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [StringLength(36)]
        public virtual string TenantId { get; protected set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(64)]
        public virtual string Name { get; protected set; }

        [Required]
        [StringLength(64)]
        public virtual string ProviderName { get; protected set; }

        [StringLength(64)]
        public virtual string ProviderKey { get; protected set; }

        protected PermissionGrant() { }

        public PermissionGrant(long id, string name, string providerName, string providerKey, string tenantId = default)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;
            ProviderName = providerName;
            ProviderKey = providerKey;
            TenantId = tenantId;
        }
    }
}