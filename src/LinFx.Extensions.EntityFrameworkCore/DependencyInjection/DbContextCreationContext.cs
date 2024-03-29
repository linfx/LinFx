using System.Data.Common;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection;

/// <summary>
/// DbContext创建上下文
/// </summary>
public class DbContextCreationContext
{
    private static readonly AsyncLocal<DbContextCreationContext> _current = new();

    public static DbContextCreationContext Current => _current.Value;

    public string ConnectionStringName { get; }

    public string ConnectionString { get; }

    public DbConnection ExistingConnection { get; internal set; }

    public DbContextCreationContext(string connectionStringName, string connectionString)
    {
        ConnectionStringName = connectionStringName;
        ConnectionString = connectionString;
    }

    public static IDisposable Use(DbContextCreationContext context)
    {
        var previousValue = Current;
        _current.Value = context;
        return new DisposeAction(() => _current.Value = previousValue);
    }
}
