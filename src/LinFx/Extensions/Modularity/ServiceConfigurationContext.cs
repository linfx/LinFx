using LinFx.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Modularity;

public class ServiceConfigurationContext(IServiceCollection services)
{
    public IServiceCollection Services { get; } = Check.NotNull(services, nameof(services));

    public IDictionary<string, object> Items { get; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets/sets arbitrary named objects those can be stored during
    /// the service registration phase and shared between modules.
    ///
    /// This is a shortcut usage of the <see cref="Items"/> dictionary.
    /// Returns null if given key is not found in the <see cref="Items"/> dictionary.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public object this[string key]
    {
        get => Items.GetOrDefault(key);
        set => Items[key] = value;
    }
}
