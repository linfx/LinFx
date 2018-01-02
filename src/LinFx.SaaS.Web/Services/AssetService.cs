using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using LinFx.Data.Extensions;
using LinFx.SaaS.Web.Entities;
using LinFx.Data;

namespace LinFx.SaaS.Web.Services
{
    public class AssetService : ServiceBase<Asset, long>
    {
        public override (IEnumerable<Asset> Items, int Total, int Count) GetList(IDictionary<string, string> filter, Paging paging = null, params Sorting[] sorting)
        {
            var sql = new StringBuilder("select * from asset");

            if(paging != null && paging.Page > 0)
            {
                sql.Append(string.Format(" limit {0} {1}", (paging.Page - 1) * paging.Limit, paging.Limit));
            }

            var items = _db.Query<Asset>(sql.ToString());
            var total = _db.ExecuteScalar<int>("select count(1) from asset");

            return (items, total, items.Count());
        }
    }
}