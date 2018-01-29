using System.Collections.Generic;
using LinFx.Domain.Entities;
using LinFx.Data;
using LinFx.Data.Extensions;
using System.Linq;
using LinFx.Data.Expressions;
using System;
using LinFx.Caching;

namespace LinFx.Domain.Services
{
    public interface IDomainService<TEntity, TPrimaryKey> : IDisposable
    {
        void Insert(ref TEntity item);
        void Update(ref TEntity item);
        void Delete(TEntity item);
        TEntity Get(TPrimaryKey id);
        (IEnumerable<TEntity> items, int total, int count) GetList(IDictionary<string, string> filter, Paging paging, params Sorting[] sorting);
    }

    public abstract class DomainService : LinFxServiceBase, IDisposable
    {
        protected IDatabase _db;
        protected ICache _cache;

        //public string GetConnectionString()
        //{
        //    IConfigurationBuilder builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json");

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }
    }

    public abstract class DomainService<TEntity, TPrimaryKey> : DomainService, IDomainService<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public virtual void Insert(ref TEntity item)
        {
            _db.Insert(item);
        }

        public virtual void Update(ref TEntity item)
        {
            _db.Update(item);
        }

        public virtual void Delete(TEntity item)
        {
            _db.Delete(item);
        }

        public virtual TEntity Get(TPrimaryKey id)
        {
            return _db.Get<TEntity>(id);
        }

        public virtual (IEnumerable<TEntity> items, int total, int count) GetList(IDictionary<string, string> filter, Paging paging, params Sorting[] sorting)
        {
            var where = PredicateBuilder<TEntity>.Where();

            //var map = _db.GetMap<TEntity>();
            //foreach (var item in filter)
            //{
            //    Expression<Func<Goods, bool>> current = p => p.Store_Id == store_id;
            //    var tmp = map.Properties.FirstOrDefault(p => p.Name.ToLower() == item.Key.ToLower());
            //}

            var items = _db.Select(where.Predicate, paging);
            return (items, _db.Count(where.Predicate), items.Count());
        }
    }
}