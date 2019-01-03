using LinFx.Domain.Models;

namespace LinFx.Domain.Abstractions
{
    public interface IRepository<T> : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
