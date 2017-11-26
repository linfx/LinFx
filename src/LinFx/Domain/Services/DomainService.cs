using System.Collections.Generic;
using LinFx.Data.Extensions;
using LinFx.Domain.Entities;

namespace LinFx.Domain.Services
{
    public interface IDataService<TEntity, TPrimaryKey>
    {
        void Insert(TEntity item);
        void Update(TEntity item);
        void Delete(TPrimaryKey id);
        TEntity Get(TPrimaryKey id);
        (IEnumerable<TEntity> Items, int Total, int Count) GetList(IDictionary<string, string> filter, Paging paging, params Sorting[] sorting);
    }

    public interface IDataService<TEntity> : IDataService<TEntity, string>
    {
    }

    public interface IDomainService
    {
    }

	public abstract class DomainService : LinFxServiceBase, IDomainService
	{
	}

    public abstract class DomainService<TEntity, TPrimaryKey> : DomainService where TEntity : IEntity<TPrimaryKey>
    {
        //protected readonly IRepository<TEntity, TPrimaryKey> _repository;

        //public DomainService(IRepository<TEntity, TPrimaryKey> repository)
        //{
        //    _repository = repository;
        //}

        //public virtual void Create(TEntity item)
        //{
        //    _repository.Insert(item);
        //}

        //public virtual void Update(TEntity item)
        //{
        //    _repository.Update(item);
        //}

        //public virtual void Delete(TEntity item)
        //{
        //    _repository.Delete(item);
        //}

        //public virtual TEntity Get(TPrimaryKey id)
        //{
        //    return _repository.Get(id);
        //}

        //public virtual IEnumerable<TEntity> GetAll()
        //{
        //    return _repository.GetAll();
        //}
    }

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