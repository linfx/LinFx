using System;
using System.Collections.Generic;

namespace LinFx.Application.ViewModels
{
    /// <summary>
    /// This interface is defined to standardize to set "Total Count of Items" to a DTO.
    /// </summary>
    public interface IHasTotalCount
    {
        /// <summary>
        /// Total count of Items.
        /// </summary>
        int TotalCount { get; set; }
    }

    /// <summary>
    /// This interface is defined to standardize to return a page of items to clients.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="IListResult{T}.Items"/> list</typeparam>
    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {
    }

    /// <summary>
    /// Implements <see cref="IPagedResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="ListResult{T}.Items"/> list</typeparam>
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
        public PagedInfo(int totalCount, IReadOnlyList<T> items) : base(items)
        {
            TotalCount = totalCount;
        }

        /// <summary>
        /// Total count of Items.
        /// </summary>
        public int TotalCount { get; set; }
    }

    [Obsolete("Use PagedInfo<T>")]
    public class PagedResultDto<T> : ListResult<T>, IPagedResult<T>
    {
        /// <summary>
        /// Creates a new <see cref="PagedInfo{T}"/> object.
        /// </summary>
        public PagedResultDto() { }

        /// <summary>
        /// Creates a new <see cref="PagedInfo{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PagedResultDto(int totalCount, IReadOnlyList<T> items) : base(items)
        {
            TotalCount = totalCount;
        }

        /// <summary>
        /// Total count of Items.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
