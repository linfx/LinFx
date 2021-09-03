using LinFx.Domain.Entities;

namespace LinFx.Domain.Repositories
{
    public interface IReadOnlyBasicRepository<TEntity> where TEntity : class, IEntity
    {
    }

    public interface IReadOnlyBasicRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
    }
}