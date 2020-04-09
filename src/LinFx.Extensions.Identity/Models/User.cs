using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : User<string>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [StringLength(36)]
        public override string Id { get; set; }
    }

    /// <summary>
    /// 用户
    /// </summary>
    /// <typeparam name="TKey">The type used from the primary key for the user.</typeparam>
    public class User<TKey> : IdentityUser<TKey>, IEntity<TKey> where TKey : IEquatable<TKey>
    {
    }
}
