using JetBrains.Annotations;

namespace LinFx.Extensions.Features;

public interface IFeatureChecker
{
    Task<string?> GetOrNullAsync([NotNull] string name);

    Task<bool> IsEnabledAsync(string name);
}
