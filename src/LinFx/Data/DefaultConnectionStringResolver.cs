using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LinFx.Data;

[Service]
public class DefaultConnectionStringResolver : IConnectionStringResolver
{
    protected DbConnectionOptions Options { get; }

    public DefaultConnectionStringResolver(
        IOptionsSnapshot<DbConnectionOptions> options)
    {
        Options = options.Value;
    }

    public virtual Task<string> ResolveAsync(string connectionStringName = null)
    {
        return Task.FromResult(ResolveInternal(connectionStringName));
    }

    private string ResolveInternal(string connectionStringName)
    {
        if (connectionStringName == null)
            return Options.ConnectionStrings.Default;

        var connectionString = Options.GetConnectionStringOrNull(connectionStringName);

        if (!connectionString.IsNullOrEmpty())
            return connectionString;

        return null;
    }
}
