using LinFx.Domain.Entities.Auditing;

namespace LinFx.SaaS.Authorization.Roles
{
    public class Role : FullAuditedEntity
    {
        /// <summary>
        /// Tenant's Id, if this role is a tenant-level role. Null, if not.
        /// </summary>
        public string TenantId { get; set; }
        /// <summary>
        /// Unique name of this role.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Display name of this role.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Is this a static role?
        /// Static roles can not be deleted, can not change their name.
        /// They can be used programmatically.
        /// </summary>
        public bool IsStatic { get; set; }
        /// <summary>
        /// Is this role will be assigned to new users as default?
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
