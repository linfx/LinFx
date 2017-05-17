//using LinFx.Data;
//using LinFx.Domain.Entities;
//using System.Threading.Tasks;

//namespace LinFx.Domain.Services
//{
//    public abstract class DomainService<TEntity, TPrimaryKey> : IDomainService where TEntity : IEntity<TPrimaryKey>
//    {
//        protected readonly IRepository<TEntity, TPrimaryKey> _repository;

//        public DomainService(IRepository<TEntity, TPrimaryKey> repository)
//        {
//            _repository = repository;
//        }

//        public virtual Task CreateAsync(TEntity item)
//        {
//            return _repository.InsertAsync(item);
//        }

//        public virtual Task UpdateAsync(TEntity item)
//        {
//            return _repository.UpdateAsync(item);
//        }

//        public virtual Task DeleteAsync(TEntity item)
//        {
//            return _repository.DeleteAsync(item);
//        }

//        public virtual Task<TEntity> GetAsync(TPrimaryKey id)
//        {
//            return _repository.GetAsync(id);
//        }
//    }

//    public abstract class DomainService<TEntity> : DomainService<TEntity, string> where TEntity : IEntity
//    {
//        public DomainService(IRepository<TEntity, string> repository) : base(repository) { }

//        public override Task CreateAsync(TEntity item)
//        {
//            item.NewId();
//            return base.CreateAsync(item);
//        }

//        public virtual Task<TEntity> FindByIdAsync(string id)
//        {
//            return _repository.FirstOrDefaultAsync(id);
//        }
//    }
//}