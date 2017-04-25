using LinFx.Authorization.Accounts;
using LinFx.Domain.Entities.Auditing;

namespace LinFx.SaaS.Authorization.Users
{
    public class User : FullAuditedEntity
    {
        public string TenantId { get; set; }

        public string Name { get; set; }

        public virtual Account Account { get; set; }
    }
}