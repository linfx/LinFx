using LinFx.Domain.Entities.Auditing;

namespace LinFx.SaaS.Web.Entities
{
    public class Article : FullAuditedEntity<int>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}