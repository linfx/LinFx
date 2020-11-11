using LinFx.Domain.Entities.Auditing;
using System;

namespace LinFx.Extensions.Blogging.Domain.Blogs
{
    /// <summary>
    /// 博客
    /// </summary>
    public class Blog : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 博客名称
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// 博客简称
        /// </summary>
        public virtual string ShortName { get; protected set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }
    }
}
