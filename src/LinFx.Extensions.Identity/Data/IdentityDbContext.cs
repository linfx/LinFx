using LinFx.Extensions.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.Identity.Data
{
    public class IdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }
    }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class IdentityDbContext<TUser, TRole> : IdentityDbContext<TUser, TRole, string, IdentityUserClaim, IdentityUserRole, IdentityUserLogin, IdentityRoleClaim, IdentityUserToken>
        where TUser : IdentityUser
        where TRole : IdentityRole
    {
        public IdentityDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            CustomModelBuilder(builder);
        }

        protected virtual void CustomModelBuilder(ModelBuilder builder)
        {
            builder.Entity<TUser>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<TRole>().ToTable(TableConsts.IdentityRoles);
            builder.Entity<IdentityUserRole>().ToTable(TableConsts.IdentityUserRoles);
            builder.Entity<IdentityRoleClaim>().ToTable(TableConsts.IdentityRoleClaims);
            builder.Entity<IdentityUserLogin>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<IdentityUserClaim>().ToTable(TableConsts.IdentityUserClaims);
            builder.Entity<IdentityUserToken>().ToTable(TableConsts.IdentityUserTokens);
        }
    }
}
