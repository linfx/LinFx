using LinFx.Extensions.Data;
using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.Account.EntityFrameworkCore;

[ConnectionStringName("Default")]
public class AccountDbContext : EfDbContext
{
    public AccountDbContext() { }

    public AccountDbContext(DbContextOptions options)
        : base(options) { }
}