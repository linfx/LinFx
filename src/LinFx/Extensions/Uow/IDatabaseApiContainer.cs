using System.Diagnostics.CodeAnalysis;
using LinFx.Extensions.DependencyInjection;

namespace LinFx.Extensions.Uow;

public interface IDatabaseApiContainer : IServiceProviderAccessor
{
    IDatabaseApi FindDatabaseApi([NotNull] string key);

    void AddDatabaseApi([NotNull] string key, [NotNull] IDatabaseApi api);

    IDatabaseApi GetOrAddDatabaseApi([NotNull] string key, [NotNull] Func<IDatabaseApi> factory);
}