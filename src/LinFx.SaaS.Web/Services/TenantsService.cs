using System.Collections.Generic;
using System.Linq;
using LinFx.Data.Extensions;
using LinFx.SaaS.MultiTenancy;

namespace LinFx.SaaS.Web.Services
{
    public class TenantService : ServiceBase<Tenant>
    {
        public override (IEnumerable<Tenant> Items, int Total, int Count) GetList(IDictionary<string, string> filter, Paging paging = null, params Sorting[] sorting)
        {
            string sql = "select * from tenant";

            var items = _db.Query<Tenant>(sql);
            var total = _db.ExecuteScalar<int>("select count(1) from tenant");

            return (items, total, items.Count());
        }
    }
}