using LinFx.Application.Abstractions;
using System.Collections.Generic;

namespace LinFx.Application.Models
{
    /// <summary>
    /// Implements <see cref="IPagedResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="ListResult{T}.Items"/> list</typeparam>
    public class PagedResult<T> : ListResult<T>, IPagedResult<T>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public long TotalCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        /// <summary>
        /// Creates a new <see cref="PagedResult{T}"/> object.
        /// </summary>
        public PagedResult() { }

        /// <summary>
        /// Creates a new <see cref="PagedResult{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PagedResult(long totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }
}