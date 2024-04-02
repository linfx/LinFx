using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Features;

public interface IFeatureStore
{
    Task<string?> GetOrNullAsync([NotNull] string name, string? providerName, string? providerKey);
}
