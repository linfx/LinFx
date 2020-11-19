using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace LinFx.Extensions.Identity
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : User<long> { }

    /// <summary>
    /// 用户
    /// </summary>
    /// <typeparam name="TKey">The type used from the primary key for the user.</typeparam>
    public class User<TKey> : IdentityUser<TKey>, IEntity<TKey> where TKey : IEquatable<TKey> { }
}
