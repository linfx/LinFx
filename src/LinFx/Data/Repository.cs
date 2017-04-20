using LinFx.Domain.Entities;
using System.Threading.Tasks;

namespace LinFx.Data
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity<string>
    {
        protected IDbConnectionFactory _factory;

        public RepositoryBase(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public abstract Task InsertAsync(TEntity item);

        public abstract Task UpdateAsync(TEntity item);

        public abstract Task DeleteAsync(TEntity item);

        public abstract Task DeleteAsync(string id);

        public abstract Task<TEntity> FirstOrDefaultAsync(string id);

        public abstract Task<TEntity> GetAsync(string id);
    }
}
