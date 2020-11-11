using LinFx;
using LinFx.Domain;
using LinFx.Domain.Entities;
using LinFx.Extensions.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinFx.Domain.Entities.Auditing
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
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
