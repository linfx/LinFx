using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace LinFx.Extensions.Identity
{
    public class UserRole : UserRole<long> { }

    public class UserRole<TKey> : IdentityUserRole<TKey>, IEntity where TKey : IEquatable<TKey> { }
}
