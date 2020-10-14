using LinFx.Domain.Models.Auditing;
using LinFx.Extensions.Auditing;
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

        public virtual Guid? RepliedCommentId { get; protected set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Text { get; protected set; }
    }
}
