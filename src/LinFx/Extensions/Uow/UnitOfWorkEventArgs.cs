namespace LinFx.Extensions.Uow;

public class UnitOfWorkEventArgs(IUnitOfWork unitOfWork) : EventArgs
{
    /// <summary>
    /// Reference to the unit of work related to this event.
    /// </summary>
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;
}