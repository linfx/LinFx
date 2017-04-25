using LinFx.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinFx.SaaS.Features
{
    /// <summary>
    /// Defines a feature of the application. A <see cref="Feature"/> can be used in a multi-tenant application
    /// to enable disable some application features depending on editions and tenants.
    /// </summary>
    [Table("Feature")]
    public class FeatureSetting : CreationAuditedEntity
    {
        /// <summary>
        /// Feature name.
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Value.
        /// </summary>
        public virtual string Value { get; set; }
    }
}
