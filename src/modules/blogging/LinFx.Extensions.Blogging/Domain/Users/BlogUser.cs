using LinFx.Domain.Models;
using System;

namespace LinFx.Extensions.Blogging.Domain.Models
{
    /// <summary>
    /// 博主
    /// </summary>
    public class BlogUser : AggregateRoot<Guid>
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; protected set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; protected set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }

        public virtual string Surname { get; set; }

        public virtual bool EmailConfirmed { get; protected set; }

        public virtual string PhoneNumber { get; protected set; }

        public virtual bool PhoneNumberConfirmed { get; protected set; }
    }
}
