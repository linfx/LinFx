using LinFx.Extensions.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<IdentityUser>().ToTable(IdentityConsts.Users);
        builder.Entity<IdentityRole>().ToTable(IdentityConsts.Roles);
        builder.Entity<IdentityRoleClaim<string>>().ToTable(IdentityConsts.RoleClaims);
        builder.Entity<IdentityUserClaim<string>>().ToTable(IdentityConsts.UserClaims);
        builder.Entity<IdentityUserRole<string>>().ToTable(IdentityConsts.UserRoles);
        builder.Entity<IdentityUserLogin<string>>().ToTable(IdentityConsts.UserLogins);
        builder.Entity<IdentityUserToken<string>>().ToTable(IdentityConsts.UserTokens);
    }
}
