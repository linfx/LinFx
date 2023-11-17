namespace LinFx.Application.Dtos;

/// <summary>
/// Implements <see cref="IListResult{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the items in the <see cref="Items"/> list</typeparam>
[Serializable]
public class ListResult<T> : IListResult<T>
{
    /// <inheritdoc />
    public IReadOnlyList<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Creates a new <see cref="ListResult{T}"/> object.
    /// </summary>
    public ListResult() { }

    /// <summary>
    /// Creates a new <see cref="ListResult{T}"/> object.
    /// </summary>
    /// <param name="items">List of items</param>
    public ListResult(IReadOnlyList<T> items) => Items = items;
}
