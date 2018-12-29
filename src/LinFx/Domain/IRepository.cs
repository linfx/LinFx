using LinFx.Domain.Uow;

namespace LinFx.Domain
{
    public interface IRepository<T> : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
