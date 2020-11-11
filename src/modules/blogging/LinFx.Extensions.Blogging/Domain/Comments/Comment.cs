using LinFx.Domain.Entities.Auditing;
using System;

namespace LinFx.Extensions.Blogging.Domain.Models
{
    /// <summary>
    /// 评论
    /// </summary>
    public class Comment : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 帖子Id
        /// </summary>
        public virtual Guid PostId { get; protected set; }

        /// <summary>
        /// 回复Id
        /// </summary>
        public virtual Guid? RepliedCommentId { get; protected set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Text { get; protected set; }
    }
}
