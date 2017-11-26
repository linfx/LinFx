using System.Collections.Generic;
using System.Linq;
using LinFx.Data.Extensions;
using LinFx.SaaS.Web.Entities;

namespace LinFx.SaaS.Web.Services
{
    public class UserService : ServiceBase<UserInfo, long>
    {
        public override void Insert(UserInfo item)
        {
            _db.BeginTransaction();

            item.Id = _db.Insert(item.Account);

            base.Insert(item);
            _db.Commit();
        }

        public override (IEnumerable<UserInfo> Items, int Total, int Count) GetList(IDictionary<string, string> filter, Paging paging = null, params Sorting[] sorting)
        {
            //var sql = new StringBuilder("select * from userinfo");
            //if (paging != null && paging.Page > 0)
            //{
            //    sql.Append(string.Format(" limit {0} {1}", (paging.Page - 1) * paging.Limit, paging.Limit));
            //}
            //var items = _db.Query<UserInfo>(sql.ToString());
            //var total = _db.ExecuteScalar<int>("select count(1) from userinfo");

            var items = _db.Select<UserInfo>(null, paging);
            var total = _db.Count<UserInfo>();
            return (items, total, items.Count());
        }
    }
}