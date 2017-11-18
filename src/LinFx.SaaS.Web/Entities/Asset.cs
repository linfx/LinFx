using LinFx.Domain.Entities.Auditing;

namespace LinFx.SaaS.Web.Entities
{
    public class Asset : FullAuditedEntity
    {
        public string TenantId { get; set; }

        public string Name { get; set; }
    }
}
