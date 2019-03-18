using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class IdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole>
    {
        public IdentityDbContext(DbContextOptions options) : base(options) { }
    }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class IdentityDbContext<TUser, TRole> : IdentityDbContext<TUser, TRole, string>
        where TUser : Microsoft.AspNetCore.Identity.IdentityUser<string>
        where TRole : Microsoft.AspNetCore.Identity.IdentityRole<string>
    {
        public IdentityDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>(b =>
            {
                b.Property(u => u.TenantId).HasMaxLength(32);
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.Property(u => u.TenantId).HasMaxLength(32);
            });
        }
    }
}
