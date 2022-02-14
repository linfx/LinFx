using LinFx.Extensions.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore;

public static class SqliteModelBuilderExtensions
{
    public static void UseSqlite(
        this ModelBuilder modelBuilder)
    {
        modelBuilder.SetDatabaseProvider(EfDatabaseProvider.Sqlite);
    }
}
