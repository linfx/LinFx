using LinFx.Domain.Entities.Auditing;

namespace LinFx.SaaS.Domain.Entities
{
	public class OrganizationUnit : FullAuditedEntity
	{
		public int TenantId { get; set; }

		public int ParentId { get; set; }

		public string No { get; set; }

		public string Name { get; set; }

		public string Note { get; set; }
    }
}
