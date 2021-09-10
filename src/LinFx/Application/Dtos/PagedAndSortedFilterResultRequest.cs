using System;
using System.Linq.Expressions;

namespace LinFx.Application.Models
{
    public class PagedAndSortedFilterResultRequest : PagedAndSortedResultRequest, IFilterRequest
    {
        /// <summary>
        ///过滤
        /// </summary>
        public string Filter { get; set; }
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

    [Obsolete]
    public class FilterPagedAndSortedResultRequest : PagedAndSortedResultRequest, IFilterRequest
    {
        /// <summary>
        ///过滤
        /// </summary>
        public string Filter { get; set; }
    }

    [Obsolete]
    public class FilterPagedAndSortedResultRequest<T> : FilterPagedAndSortedResultRequest
    {
        public FilterPagedAndSortedResultRequest(FilterPagedAndSortedResultRequest request, Expression<Func<T, bool>> expression)
        {
            Filter = request.Filter;
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
}
