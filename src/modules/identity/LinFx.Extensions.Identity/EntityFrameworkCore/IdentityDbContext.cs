using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace LinFx.Extensions.Identity.EntityFrameworkCore
{
    public class IdentityDbContext : IdentityDbContext<User, Role>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }
    }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class IdentityDbContext<TUser, TRole> : IdentityDbContext<TUser, TRole, long>
        where TUser : User
        where TRole : Role
    {
        public IdentityDbContext(DbContextOptions options) : base(options) { }
    }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class IdentityDbContext<TUser, TRole, TKey> : IdentityDbContext<TUser, TRole, TKey, UserClaim<TKey>, UserRole<TKey>, UserLogin<TKey>, RoleClaim<TKey>, UserToken<TKey>>
        where TUser : User<TKey>
        where TRole : Role<TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            CustomModelBuilder(builder);
        }

        protected virtual void CustomModelBuilder(ModelBuilder builder)
        {
            builder.Entity<TUser>().ToTable(TableConsts.Users);
            builder.Entity<TRole>().ToTable(TableConsts.Roles);
            builder.Entity<UserRole>().ToTable(TableConsts.UserRoles);
            builder.Entity<RoleClaim>().ToTable(TableConsts.RoleClaims);
            builder.Entity<UserLogin>().ToTable(TableConsts.UserLogins);
            builder.Entity<UserClaim>().ToTable(TableConsts.UserClaims);
            builder.Entity<UserToken>().ToTable(TableConsts.UserTokens);
        }
    }
}
