using System;
using JetBrains.Annotations;

namespace LinFx.Extensions.Modularity;

public interface IDependedTypesProvider
{
    [NotNull]
    Type[] GetDependedTypes();
}
