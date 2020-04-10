using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Models
{
    public class UserLogin : UserLogin<string>
    {
        [StringLength(36)]
        public override string UserId { get; set; }
    }

    public class UserLogin<TKey> : IdentityUserLogin<TKey>, IEntity where TKey : IEquatable<TKey>
    {
        public override TKey UserId { get; set; }
    }
}
