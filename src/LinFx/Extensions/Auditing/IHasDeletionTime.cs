using System;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// A standard interface to add DeletionTime property to a class.
/// It also makes the class soft delete (see <see cref="ISoftDelete"/>).
/// </summary>
public interface IHasDeletionTime : ISoftDelete
{
    /// <summary>
    /// Deletion time.
    /// </summary>
    DateTimeOffset? DeletionTime { get; set; }
}
