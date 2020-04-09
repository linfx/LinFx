using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Models
{
    public class UserToken : UserToken<string>
    {
        [StringLength(36)]
        public override string UserId { get; set; }
    }

    public class UserToken<TKey> : IdentityUserToken<TKey>, IEntity where TKey : IEquatable<TKey>
    {
        public override TKey UserId { get; set; }
    }
}
