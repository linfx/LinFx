using LinFx.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinFx.Domain.Models.Auditing
{
    /// <summary>
    /// 审计实体
    /// </summary>
    public abstract class EntityAudited : EntityAudited<long> { }

    /// <summary>
    /// 审计实体
    /// </summary>
    public abstract class EntityAudited<TKey> : Entity<TKey>, ISoftDelete
    {
        /// <summary>
        /// 是否已删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 更新时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTimeOffset? UpdatedOn { get; set; }
    }
}
