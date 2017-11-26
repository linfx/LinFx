using LinFx.Authorization.Accounts;
using LinFx.Domain.Entities.Auditing;

namespace LinFx.SaaS.Authorization.Users
{
    public class User : FullAuditedEntity<long>
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public virtual Account Account { get; set; }
    }
}