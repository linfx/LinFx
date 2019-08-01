using LinFx;
using LinFx.Application.Abstractions;
using LinFx.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace System.Linq
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
            return new PagedResult<T>(total, await items.ToListAsync());
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
            return new PagedResult<T>(total, await items.ToListAsync());
        }
    }
}