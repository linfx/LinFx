using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace LinFx.Extensions.Identity
{
    public class UserClaim : UserClaim<string> { }

    public class UserClaim<TKey> : IdentityUserClaim<TKey>, IEntity where TKey : IEquatable<TKey> { }
}
