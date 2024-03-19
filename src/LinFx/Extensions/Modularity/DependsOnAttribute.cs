using JetBrains.Annotations;

namespace LinFx.Extensions.Modularity;

/// <summary>
/// Used to define dependencies of a type.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependsOnAttribute(params Type[] dependedTypes) : Attribute, IDependedTypesProvider
{
    [NotNull]
    public Type[] DependedTypes { get; } = dependedTypes ?? new Type[0];

    public virtual Type[] GetDependedTypes() => DependedTypes;
}
