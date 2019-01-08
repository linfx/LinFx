using LinFx.Domain.Models;

namespace LinFx.Domain.Abstractions
{
    /// <summary>
    /// 仓储泛型接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
