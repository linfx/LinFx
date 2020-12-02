using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace LinFx.Extensions.Identity
{
    public class IdentityDbContext : IdentityDbContext<User, Role>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityDbContext"/>.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public IdentityDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityDbContext" /> class.
        /// </summary>
        protected IdentityDbContext() { }
    }

    public class IdentityDbContext<TUser> : IdentityDbContext<TUser, Role>
        where TUser : User
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityDbContext"/>.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public IdentityDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityDbContext" /> class.
        /// </summary>
        protected IdentityDbContext() { }
    }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class IdentityDbContext<TUser, TRole> : IdentityDbContext<TUser, TRole, string>
        where TUser : User
        where TRole : Role
    {
        /// <summary>
        /// Initializes a new instance of the db context.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public IdentityDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected IdentityDbContext() { }
    }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class IdentityDbContext<TUser, TRole, TKey> : IdentityDbContext<TUser, TRole, TKey, UserClaim<TKey>, UserRole<TKey>, UserLogin<TKey>, RoleClaim<TKey>, UserToken<TKey>>
        where TUser : User<TKey>
        where TRole : Role<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the db context.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public IdentityDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected IdentityDbContext() { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            CustomModelBuilder(builder);
        }

        protected virtual void CustomModelBuilder(ModelBuilder builder)
        {
            builder.Entity<TUser>().ToTable(TableConsts.Users);
            builder.Entity<TRole>().ToTable(TableConsts.Roles);
            builder.Entity<RoleClaim<TKey>>().ToTable(TableConsts.RoleClaims);
            builder.Entity<UserClaim<TKey>>().ToTable(TableConsts.UserClaims);
            builder.Entity<UserRole<TKey>>().ToTable(TableConsts.UserRoles);
            builder.Entity<UserLogin<TKey>>().ToTable(TableConsts.UserLogins);
            builder.Entity<UserToken<TKey>>().ToTable(TableConsts.UserTokens);
        }
    }
}
