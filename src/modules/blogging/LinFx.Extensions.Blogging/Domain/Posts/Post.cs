using LinFx.Domain.Models.Auditing;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Blogging.Domain.Posts
{
    /// <summary>
    /// 帖子
    /// </summary>
    public class Post : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 博客ID
        /// </summary>
        public virtual Guid BlogId { get; protected set; }

        /// <summary>
        /// 网址
        /// </summary>
        [Required]
        public virtual string Url { get; protected set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public virtual string CoverImage { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        public virtual string Title { get; protected set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        public virtual string Content { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 阅读数
        /// </summary>
        public virtual int ReadCount { get; protected set; }

        /// <summary>
        /// 标签集合
        /// </summary>
        public virtual Collection<PostTag> Tags { get; protected set; }

        /// <summary>
        /// 阅读数增长
        /// </summary>
        /// <returns></returns>
        public virtual Post IncreaseReadCount()
        {
            ReadCount++;
            return this;
        }

        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="tagId"></param>
        public virtual void AddTag(Guid tagId)
        {
            Tags.Add(new PostTag(Id, tagId));
        }

        /// <summary>
        /// 移除标签
        /// </summary>
        /// <param name="tagId"></param>
        public virtual void RemoveTag(Guid tagId)
        {
            //Tags.RemoveAt(t => t.TagId == tagId);
        }
    }
}
