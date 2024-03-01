namespace LinFx;

/// <summary>
/// Used to represent a named type selector.
/// </summary>
/// <remarks>
/// Creates new <see cref="NamedTypeSelector"/> object.
/// </remarks>
/// <param name="name">Name</param>
/// <param name="predicate">Predicate</param>
public class NamedTypeSelector(string name, Func<Type, bool> predicate)
{
    /// <summary>
    /// Name of the selector.
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// Predicate.
    /// </summary>
    public Func<Type, bool> Predicate { get; set; } = predicate;
}
