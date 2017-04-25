using LinFx.Domain.Entities.Auditing;

namespace LinFx.SaaS.Editions
{
    /// <summary>
    /// 版本
    /// </summary>
    public class Edition : FullAuditedEntity
    {
        /// <summary>
        /// Unique name of this edition.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Display name of this edition.
        /// </summary>
        public string DisplayName { get; set; }
    }
}