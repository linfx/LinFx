using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.Configuration
{
    public class EFConfigurationDbContext : DbContext
    {
        public EFConfigurationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppSetting> AppSettings { get; set; }
    }
}
