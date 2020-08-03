using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Models
{
    public class UserClaim : UserClaim<string>
    {
        [StringLength(36)]
        public override string UserId { get; set; }
    }

    public class UserClaim<TKey> : IdentityUserClaim<TKey>, IEntity where TKey : IEquatable<TKey>
    {
    }
}
