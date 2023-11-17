using JetBrains.Annotations;
using LinFx.Application.Dtos;
using LinFx.Utils;

namespace System.Linq;

public static class PagingQueryableExtensions
{
    /// <summary>
    /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="request">分页请求</param>
    /// <returns></returns>
    public static IQueryable<T> PageBy<T>([NotNull] this IQueryable<T> query, IPagedResultRequest request)
    {
        Check.NotNull(query, nameof(request));

        return PageBy(query, request.Page, request.PageSize);
    }

    /// <summary>
    /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="page">页数(eg: 1)</param>
    /// <param name="limit">页大小</param>
    /// <returns></returns>
    public static IQueryable<T> PageBy<T>([NotNull] this IQueryable<T> query, int page, int limit)
    {
        Check.NotNull(query, nameof(query));

        if (page < 1)
            page = 1;

        return query.Skip((page - 1) * limit).Take(limit);
    }

    /// <summary>
    /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
    /// </summary>
    public static TQueryable PageBy<T, TQueryable>([NotNull] this TQueryable query, int page, int limit) where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        if (page < 1)
            page = 1;

        return (TQueryable)query.Skip(page).Take(limit);
    }
}
