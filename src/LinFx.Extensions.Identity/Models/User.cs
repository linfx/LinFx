using LinFx.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : User<string>
    {
        [StringLength(36)]
        public override string Id { get; set; }
    }

    /// <summary>
    /// 用户
    /// </summary>
    /// <typeparam name="TKey">The type used from the primary key for the user.</typeparam>
    public class User<TKey> : Microsoft.AspNetCore.Identity.IdentityUser<TKey>, IEntity<TKey> where TKey : IEquatable<TKey>
    {
    }
}
