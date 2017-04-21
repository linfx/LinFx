using System;

namespace LinFx.Domain.Uow
{
    public interface IActiveUnitOfWork
    {
        /// <summary>
        /// This event is raised when this UOW is successfully completed.
        /// </summary>
        event EventHandler Completed;

        /// <summary>
        /// This event is raised when this UOW is disposed.
        /// </summary>
        event EventHandler Disposed;

        /// <summary>
        /// Gets if this unit of work is transactional.
        /// </summary>
        UnitOfWorkOptions Options { get; }

        /// <summary>
        /// Is this UOW disposed?
        /// </summary>
        bool IsDisposed { get; }
    }
}
