using LinFx.Domain.Models.Auditing;
using LinFx.Extensions.Auditing;
using System;

namespace LinFx.Extensions.Blogging.Domain.Models
{
    /// <summary>
    /// 标签
    /// </summary>
    public class Tag : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 博客Id
        /// </summary>
        public virtual Guid BlogId { get; protected set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// 描述 
        /// </summary>
        public virtual string Description { get; protected set; }

        /// <summary>
        /// 使用数量
        /// </summary>
        public virtual int UsageCount { get; protected internal set; }
    }
}
