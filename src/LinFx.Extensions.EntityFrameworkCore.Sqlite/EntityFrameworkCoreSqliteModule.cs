using LinFx.Extensions.Modularity;

namespace LinFx.Extensions.EntityFrameworkCore.Sqlite;

[DependsOn(
    typeof(EntityFrameworkCoreModule)
)]
public class EntityFrameworkCoreSqliteModule : Module
{
}
