using EfPostgresql.Domain;
using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public DbSet<SensorStat> Sensors { get; set; }

    public DbSet<PointTest> PointTests { get; set; }

    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.HasPostgresExtension("timescaledb");

        modelBuilder.Entity<SensorStat>(b =>
        {
            b.HasKey(p => new { p.Sid, p.CtrTime });
        });
    }
}
