using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Features;

public interface IFeatureChecker
{
    Task<string?> GetOrNullAsync([NotNull] string name);

    Task<bool> IsEnabledAsync(string name);
}
