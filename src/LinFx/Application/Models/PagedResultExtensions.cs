using System.Linq;

namespace LinFx.Application.Models
{
    public static class PagedResultExtensions
    {
        public static PagedResult<T> ToPageResult<T>([NotNull] this IQueryable<T> query, int page, int limit)
        {
            var total = query.LongCount();
            var items = query.PageBy(page, limit);
            return new PagedResult<T>(page, limit, total, items.ToList());
        }
    }
}