using LinFx.Data;
using LinFx.Domain.Entities;
using System.Threading.Tasks;

namespace LinFx.Domain.Repositories
{
    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, string>, IRepository<TEntity> where TEntity : class, IEntity<string>
    {
        public RepositoryBase(IDbConnectionFactory factory)
            : base(factory)
        {
        }
    }

    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        protected IDbConnectionFactory _factory;

        public RepositoryBase(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public abstract Task InsertAsync(TEntity item);

        public abstract Task UpdateAsync(TEntity item);

        public abstract Task DeleteAsync(TEntity item);

        public abstract Task DeleteAsync(TPrimaryKey id);

        public abstract Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        public abstract Task<TEntity> GetAsync(TPrimaryKey id);
    }
}