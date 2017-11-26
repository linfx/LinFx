using LinFx.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinFx.SaaS.Authorization
{
    /// <summary>
    /// Used to grant/deny a permission for a role or user.
    /// </summary>
    [Table("Permission")]
    public class PermissionSetting : CreationAuditedEntity
    {
        public int TenantId { get; set; }
        /// <summary>
        /// Unique name of the permission.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Is this role granted for this permission.
        /// Default value: true.
        /// </summary>
        public bool IsGranted { get; set; }
    }
}
