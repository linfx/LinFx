using LinFx.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace LinFx.SaaS.Configuration
{
    public class Setting : Entity
    {
        /// <summary>
        /// TenantId for this setting.
        /// TenantId is null if this setting is not Tenant level.
        /// </summary>
        public virtual string TenantId { get; set; }
        /// <summary>
        /// UserId for this setting.
        /// UserId is null if this setting is not user level.
        /// </summary>
        public virtual string UserId { get; set; }
        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public virtual string Name { get; set; }
        /// <summary>
        /// Value of the setting.
        /// </summary>
        [MaxLength(2000)]
        public virtual string Value { get; set; }
    }

    public class OrganizationUnitSettings
    {
    }
}
