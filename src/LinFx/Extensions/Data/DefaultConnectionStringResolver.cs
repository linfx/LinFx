using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.Data;

/// <summary>
/// 默认连接字符串解析器
/// </summary>
public class DefaultConnectionStringResolver : IConnectionStringResolver, ITransientDependency
{
    protected DbConnectionOptions Options { get; }

    public DefaultConnectionStringResolver(IOptionsSnapshot<DbConnectionOptions> options)
    {
        Options = options.Value;
    }

    public virtual Task<string> ResolveAsync(string? connectionStringName = null) => Task.FromResult(ResolveInternal(connectionStringName));

    private string ResolveInternal(string? connectionStringName)
    {
        if (connectionStringName == null)
            return Options.ConnectionStrings.Default;

        var connectionString = Options.GetConnectionStringOrNull(connectionStringName);

        if (!string.IsNullOrEmpty(connectionString))
            return connectionString;

        throw new ArgumentNullException("ConnectionString");
    }
}
