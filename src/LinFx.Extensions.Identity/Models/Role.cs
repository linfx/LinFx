using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace LinFx.Extensions.Identity.Models
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role : Role<string>
    {
        public Role() { }

        public Role(string roleName) : base(roleName) { }
    }

    /// <summary>
    /// 角色
    /// </summary>
    public class Role<TKey> : IdentityRole<TKey>, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public Role() { }

        public Role(string roleName) : base(roleName) { }
    }
}
