using LinFx.Domain.Entities.Auditing;

namespace LinFx.SaaS.Domain.Entities
{
	public class OrganizationUnit : FullAuditedEntity
	{
		public string TenantId { get; set; }

		public string ParentId { get; set; }

		public string No { get; set; }

		public string Name { get; set; }
    }
}
