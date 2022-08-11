using System.Linq.Expressions;

namespace LinFx.Application.Dtos;

public class PagedAndSortedFilterResultRequest : PagedAndSortedResultRequest, IFilterRequest
{
    /// <summary>
    ///过滤
    /// </summary>
    public string Filter { get; set; } = string.Empty;
}

public class FilterPagedAndSortedFilterResultRequest<T> : PagedAndSortedResultRequest
{
    public FilterPagedAndSortedFilterResultRequest(PagedAndSortedResultRequest request, Expression<Func<T, bool>> expression)
    {
        Page = request.Page;
        PageSize = request.PageSize;
        Sorting = request.Sorting;
        Expression = expression;
    }

    /// <summary>
    /// 过滤条件
    /// </summary>
    public Expression<Func<T, bool>> Expression { get; set; }
}