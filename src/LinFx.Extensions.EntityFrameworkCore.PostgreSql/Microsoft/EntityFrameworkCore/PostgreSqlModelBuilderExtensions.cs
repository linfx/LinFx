using LinFx.Extensions.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore;

public static class PostgreSqlModelBuilderExtensions
{
    public static void UsePostgreSql(this ModelBuilder modelBuilder) => modelBuilder.SetDatabaseProvider(EfDatabaseProvider.PostgreSql);
}
