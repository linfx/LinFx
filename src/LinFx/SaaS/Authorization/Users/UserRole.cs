using LinFx.Domain.Entities.Auditing;

namespace LinFx.SaaS.Domain.Entities
{
    public class UserRole : CreationAuditedEntity
    {
        public string TenantId { get; set; }
        /// <summary>
        /// User id.
        /// </summary>
        public virtual long UserId { get; set; }
        /// <summary>
        /// Role id.
        /// </summary>
        public virtual int RoleId { get; set; }
    }
}
