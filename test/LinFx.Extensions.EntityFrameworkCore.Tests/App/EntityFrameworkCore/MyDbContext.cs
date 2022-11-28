using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

class MyDbContext : EfDbContext
{
    public MyDbContext(DbContextOptions options)
        : base(options)
    { }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(b =>
        {
            //b.HasKey(p => new { p.Sid, p.CtrTime });
        });
    }
}
