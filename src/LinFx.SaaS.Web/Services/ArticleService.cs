using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using LinFx.Data.Extensions;
using LinFx.SaaS.Web.Entities;

namespace LinFx.SaaS.Web.Services
{
    public class ArticleService : ServiceBase<Article, int>
    {
        public override (IEnumerable<Article> Items, int Total, int Count) GetList(IDictionary<string, string> filter, Paging paging = null, params Sorting[] sorting)
        {
            var sql = new StringBuilder("select * from article");

            if(paging != null && paging.Page > 0)
            {
                sql.Append(string.Format(" offset {0} limit {1}", (paging.Page - 1) * paging.Limit, paging.Limit));
            }

            var items = _db.Query<Article>(sql.ToString());
            var total = _db.ExecuteScalar<int>("select count(1) from article");

            return (items, total, items.Count());
        }
    }
}