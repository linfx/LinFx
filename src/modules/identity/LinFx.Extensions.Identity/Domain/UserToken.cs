using LinFx.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace LinFx.Extensions.Identity
{
    public class UserToken : UserToken<string> { }

    public class UserToken<TKey> : IdentityUserToken<TKey>, IEntity where TKey : IEquatable<TKey> { }
}
