using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Identity.Models
{
    public class RoleClaim : RoleClaim<string>
    {
        [StringLength(36)]
        public override string RoleId { get; set; }
    }

    public class RoleClaim<TKey> : IdentityRoleClaim<TKey>, IEntity where TKey : IEquatable<TKey>
    {
    }
}
