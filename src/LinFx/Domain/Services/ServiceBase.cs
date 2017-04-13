using LinFx.Data;
using LinFx.Domain.Entities;
using System.Threading.Tasks;

namespace LinFx.Domain.Services
{
    public abstract class ServiceBase<TEntity> where TEntity : IEntity
    {
        protected readonly IRepository<TEntity> _repository;

        public ServiceBase(
            IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual void InsertAsync(TEntity item)
        {
            item.NewId();
            _repository.InsertAsync(item);
        }

        public virtual void UpdateAsync(TEntity item)
        {
            _repository.UpdateAsync(item);
        }

        public virtual void DeleteAsync(TEntity item)
        {
            _repository.DeleteAsync(item);
        }

        public virtual Task<TEntity> FindByIdAsync(string id)
        {
            return _repository.FirstOrDefaultAsync(id);
        }
    }
}