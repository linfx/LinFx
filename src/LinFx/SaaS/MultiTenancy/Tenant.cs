using LinFx.Domain.Entities.Auditing;
using LinFx.SaaS.Editions;

namespace LinFx.SaaS.MultiTenancy
{
	/// <summary>
	/// 租户
	/// </summary>
	public class Tenant : FullAuditedEntity
    {
        /// <summary>
        /// Display name of the Tenant.
        /// </summary>
        public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
        public string EditionId { get; set; }
        /// <summary>
        /// Current <see cref="Edition"/> of the Tenant.
        /// </summary>
        public virtual Edition Edition { get; set; }
    }
}
