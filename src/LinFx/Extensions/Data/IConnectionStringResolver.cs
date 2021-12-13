using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LinFx.Extensions.Data;

public interface IConnectionStringResolver
{
    [NotNull]
    Task<string> ResolveAsync(string connectionStringName = null);
}
