using System;
using System.Linq.Expressions;

namespace LinFx.Application.Models
{
    public class FilterPagedAndSortedResultRequest : PagedAndSortedResultRequest, IFilterRequest
    {
        /// <summary>
        ///过滤
        /// </summary>
        public string Filter { get; set; }
    }

    public class FilterPagedAndSortedResultRequest<T> : FilterPagedAndSortedResultRequest
    {
        public FilterPagedAndSortedResultRequest(FilterPagedAndSortedResultRequest request, Expression<Func<T, bool>> expression)
        {
            Filter = request.Filter;
            Page = request.Page;
            Limit = request.Limit;
            Sorting = request.Sorting;
            Expression = expression;
        }
        
        /// <summary>
        /// 过滤条件
        /// </summary>
        public Expression<Func<T, bool>> Expression { get; set; }
    }
}
