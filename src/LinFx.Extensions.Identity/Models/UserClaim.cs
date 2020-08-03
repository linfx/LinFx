using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace LinFx.Extensions.Identity.Models
{
    public class UserClaim : UserClaim<long> { }

    public class UserClaim<TKey> : IdentityUserClaim<TKey>, IEntity where TKey : IEquatable<TKey> { }
}
