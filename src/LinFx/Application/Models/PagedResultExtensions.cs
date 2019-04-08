using LinFx.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Application.Models
{
    public static class PagedResultExtensions
    {
        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="page">当前页</param>
        /// <param name="limit">页大小</param>
        /// <returns></returns>
        public static async Task<PagedResult<T>> ToPageResultAsync<T>([NotNull] this IQueryable<T> query, int page, int limit)
        {
            var total = query.LongCount();
            var items = query.PageBy(page, limit);
            return new PagedResult<T>(page, limit, total, await items.ToListAsync());
        }

        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="request">分页请求</param>
        /// <returns></returns>
        public static async Task<PagedResult<T>> ToPageResultAsync<T>([NotNull] this IQueryable<T> query, [NotNull] IPagedResultRequest request)
        {
            Check.NotNull(request, nameof(request));

            var total = query.LongCount();
            var items = query.PageBy(request);
            return new PagedResult<T>(request, total, await items.ToListAsync());
        }
    }
}