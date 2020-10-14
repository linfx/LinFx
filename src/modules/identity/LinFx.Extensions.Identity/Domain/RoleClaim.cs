using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace LinFx.Extensions.Identity
{
    public class RoleClaim : IdentityRoleClaim<long>, IEntity { }

    public class RoleClaim<TKey> : IdentityRoleClaim<TKey>, IEntity where TKey : IEquatable<TKey>
    {
    }
}
