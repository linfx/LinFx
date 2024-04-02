using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Uow;

/// <summary>
/// Used as event arguments on <see cref="IUnitOfWork.Failed"/> event.
/// </summary>
/// <remarks>
/// Creates a new <see cref="UnitOfWorkFailedEventArgs"/> object.
/// </remarks>
public class UnitOfWorkFailedEventArgs([NotNull] IUnitOfWork unitOfWork, [AllowNull] Exception exception, bool isRolledback) : UnitOfWorkEventArgs(unitOfWork)
{
    /// <summary>
    /// Exception that caused failure. This is set only if an error occurred during <see cref="IUnitOfWork.CompleteAsync"/>.
    /// Can be null if there is no exception, but <see cref="IUnitOfWork.CompleteAsync"/> is not called.
    /// Can be null if another exception occurred during the UOW.
    /// </summary>
    [AllowNull]
    public Exception Exception { get; } = exception;

    /// <summary>
    /// True, if the unit of work is manually rolled back.
    /// </summary>
    public bool IsRolledback { get; } = isRolledback;
}