using System.Collections.Generic;
using LinFx.Data.Extensions;
using LinFx.Data.Utils;
using LinFx.Domain.Services;
using LinFx.Domain.Entities;
using LinFx.Data.Dapper.Filters.Action;

namespace LinFx.SaaS.Web.Services
{
    public class ServiceBase : LinFxServiceBase
    {
        protected IDatabase _db;
        protected IDapperActionFilterExecuter _filter;

        public ServiceBase()
        {
            _db = DbUtils.GetPostgreSqlDatabase("server=localhost;database=linfx.saas;uid=dev;pwd=123456;pooling=true;");
            _filter = new DapperActionFilterExecuter();
        }
    }

    public class ServiceBase<TEntity> : ServiceBase, IDataService<TEntity> where TEntity : class, IEntity
    {
        public virtual void Insert(TEntity item)
        {
            _filter.ExecuteCreationAuditFilter<TEntity, string>(item);
            _db.Insert(item);
        }

        public virtual void Delete(string id)
        {
            _db.Delete<TEntity>(p => p.Id == id);
        }

        public virtual void Update(TEntity item)
        {
            _filter.ExecuteModificationAuditFilter<TEntity, string>(item);
            _db.Update(item);
        }

        public virtual TEntity Get(string id)
        {
            return _db.Get<TEntity>(id);
        }

        public virtual (IEnumerable<TEntity> Items, int Total, int Count) GetList(IDictionary<string, string> filter, Paging paging = null, params Sorting[] sorting)
        {
            return (null, 0, 0);
        }
    }
}
