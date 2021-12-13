using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Threading;

namespace LinFx.Threading;

[Service(ServiceLifetime.Singleton)]
public class AsyncLocalAmbientDataContext : IAmbientDataContext
{
    private static readonly ConcurrentDictionary<string, AsyncLocal<object>> AsyncLocalDictionary = new ConcurrentDictionary<string, AsyncLocal<object>>();

    public void SetData(string key, object value)
    {
        var asyncLocal = AsyncLocalDictionary.GetOrAdd(key, (k) => new AsyncLocal<object>());
        asyncLocal.Value = value;
    }

    public object GetData(string key)
    {
        var asyncLocal = AsyncLocalDictionary.GetOrAdd(key, (k) => new AsyncLocal<object>());
        return asyncLocal.Value;
    }
}
