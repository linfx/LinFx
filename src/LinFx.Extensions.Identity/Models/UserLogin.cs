using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace LinFx.Extensions.Identity.Models
{
    public class UserLogin : UserLogin<long> { }

    public class UserLogin<TKey> : IdentityUserLogin<TKey>, IEntity where TKey : IEquatable<TKey> { }
}
