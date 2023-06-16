using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.EntityFrameworkCore
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>().ToTable(TableConsts.Users);
            builder.Entity<IdentityRole>().ToTable(TableConsts.Roles);
            builder.Entity<IdentityRoleClaim<string>>().ToTable(TableConsts.RoleClaims);
            builder.Entity<IdentityUserClaim<string>>().ToTable(TableConsts.UserClaims);
            builder.Entity<IdentityUserRole<string>>().ToTable(TableConsts.UserRoles);
            builder.Entity<IdentityUserLogin<string>>().ToTable(TableConsts.UserLogins);
            builder.Entity<IdentityUserToken<string>>().ToTable(TableConsts.UserTokens);
        }
    }
}
