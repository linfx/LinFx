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
        where TUser : IdentityUser
        where TRole : IdentityRole
    {
        public IdentityDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TUser>(b =>
            {
                b.Property(u => u.TenantId).HasMaxLength(32);
            });

            builder.Entity<TRole>(b =>
            {
                b.Property(u => u.TenantId).HasMaxLength(32);
            });
        }
    }
}
