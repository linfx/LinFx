using System.Collections.Generic;
using System.Linq;
using LinFx.Data.Extensions;
using LinFx.SaaS.Web.Entities;

namespace LinFx.SaaS.Web.Services
{
    public class UserService : ServiceBase<UserInfo>
    {
        public override (IEnumerable<UserInfo> Items, int Total, int Count) GetList(IDictionary<string, string> filter, Paging paging = null, params Sorting[] sorting)
        {
            string sql = "select * from userinfo";

            var items = _db.Query<UserInfo>(sql);
            var total = _db.ExecuteScalar<int>("select count(1) from userinfo");

            return (items, total, items.Count());
        }
    }
}