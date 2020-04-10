using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Models
{
    public class UserRole : UserRole<string>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [StringLength(36)]
        public override string UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [StringLength(36)]
        public override string RoleId { get; set; }
    }

    public class UserRole<TKey> : IdentityUserRole<TKey>, IEntity where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public override TKey UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public override TKey RoleId { get; set; }
    }
}
