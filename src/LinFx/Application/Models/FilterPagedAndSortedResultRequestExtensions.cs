using System;
using System.Linq.Expressions;

namespace LinFx.Application.Models
{
    public static class PagedAndSortedResultRequestExtensions
    {
        /// <summary>
        /// 转换类型
        /// </summary>
        /// <returns></returns>
        public static FilterPagedAndSortedResultRequest<T> ToPagedAndSortedResultRequest<T>(this FilterPagedAndSortedResultRequest request, Expression<Func<T, bool>> expression)
        {
            return new FilterPagedAndSortedResultRequest<T>(request, expression);
        }
    }
}
