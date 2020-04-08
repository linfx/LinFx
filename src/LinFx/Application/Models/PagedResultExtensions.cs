using LinFx;
using LinFx.Application;
using LinFx.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class PagedResultExtensions
    {
        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="page">当前页</param>
        /// <param name="limit">页大小</param>
        /// <returns></returns>
        public static async Task<PagedResult<TEntity>> ToPageResultAsync<TEntity>([NotNull] this IQueryable<TEntity> query, int page, int limit)
        {
            var total = query.LongCount();
            var items = query.PageBy(page, limit);
            return new PagedResult<TEntity>(page, limit, total, await items.ToListAsync());
        }

        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="request">分页请求</param>
        /// <returns></returns>
        public static async Task<PagedResult<TEntity>> ToPageResultAsync<TEntity>([NotNull] this IQueryable<TEntity> query, [NotNull] IPagedResultRequest request)
        {
            Check.NotNull(request, nameof(request));

            var total = query.LongCount();
            var items = query.PageBy(request);
            return new PagedResult<TEntity>(request, total, await items.ToListAsync());
        }

        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<PagedResult<TModel>> ToPageResultAsync<TEntity, TModel>(
            [NotNull] this IQueryable<TEntity> query,
            [NotNull] IPagedResultRequest request)
                where TModel : class
        {
            Check.NotNull(request, nameof(request));

            var total = await query.LongCountAsync();
            var items = await query.PageBy(request).ToListAsync();

            var result = new PagedResult<TModel>(request, total, items.Select(MapToListOutput).ToList());
            return result;

            static TModel MapToListOutput(TEntity entity)
            {
                return entity.MapTo<TModel>();
            }
        }
    }
}