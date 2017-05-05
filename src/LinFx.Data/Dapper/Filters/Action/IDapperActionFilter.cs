using LinFx.Domain.Entities;

namespace LinFx.Data.Dapper.Filters.Action
{
    public interface IDapperActionFilter
    {
        void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;
    }
}
