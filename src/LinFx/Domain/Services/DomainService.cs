using LinFx.Data;
using LinFx.Domain.Entities;
using System.Collections.Generic;

namespace LinFx.Domain.Services
{
    public interface IDomainService
    {
    }

	public abstract class DomainService : ServiceBase, IDomainService
	{
	}


	//public abstract class DomainService<TEntity, TPrimaryKey> : IDomainService where TEntity : IEntity<TPrimaryKey>
 //   {
 //       protected readonly IRepository<TEntity, TPrimaryKey> _repository;

 //       public DomainService(IRepository<TEntity, TPrimaryKey> repository)
 //       {
 //           _repository = repository;
 //       }

 //       //public virtual void Create(TEntity item)
 //       //{
 //       //    _repository.Insert(item);
 //       //}

 //       //public virtual void Update(TEntity item)
 //       //{
 //       //    _repository.Update(item);
 //       //}

 //       //public virtual void Delete(TEntity item)
 //       //{
 //       //    _repository.Delete(item);
 //       //}

 //       //public virtual TEntity Get(TPrimaryKey id)
 //       //{
 //       //    return _repository.Get(id);
 //       //}

 //       //public virtual IEnumerable<TEntity> GetAll()
 //       //{
 //       //    return _repository.GetAll();
 //       //}
 //   }

    //public abstract class DomainService<TEntity> : DomainService<TEntity, string> where TEntity : IEntity
    //{
    //    //public DomainService(IRepository<TEntity> repository) : base(repository) { }

    //    //public override void Create(TEntity item)
    //    //{
    //    //    item.NewId();
    //    //    base.Create(item);
    //    //}
    //}
}