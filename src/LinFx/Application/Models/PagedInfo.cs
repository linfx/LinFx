using LinFx.Application.Abstractions;
using System;
using System.Collections.Generic;

namespace LinFx.Application.Models
{
    /// <summary>
    /// Implements <see cref="IPagedResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="ListResult{T}.Items"/> list</typeparam>
    [Obsolete("Use PagedResult")]
    public class PagedInfo<T> : ListResult<T>, IPagedResult<T>
    {
        /// <summary>
        /// Creates a new <see cref="PagedInfo{T}"/> object.
        /// </summary>
        public PagedInfo() { }

        /// <summary>
        /// Creates a new <see cref="PagedInfo{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PagedInfo(long totalCount, IReadOnlyList<T> items) : base(items)
        {
            TotalCount = totalCount;
        }

        /// <summary>
        /// Total count of Items.
        /// </summary>
        public long TotalCount { get; set; }
    }
}
