using System.Collections.Generic;

namespace LinFx.Application.Models
{
    /// <summary>
    /// Implements <see cref="IListResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="Items"/> list</typeparam>
    public class ListResult<T> : IListResult<T>
    {
        private IReadOnlyList<T> _items;

        /// <summary>
        /// Creates a new <see cref="ListResult{T}"/> object.
        /// </summary>
        public ListResult() { }

        /// <summary>
        /// Creates a new <see cref="ListResult{T}"/> object.
        /// </summary>
        /// <param name="items">List of items</param>
        public ListResult(IReadOnlyList<T> items)
        {
            Items = items;
        }

        /// <summary>
        /// List of items.
        /// </summary>
        public IReadOnlyList<T> Items
        {
            get { return _items ?? (_items = new List<T>()); }
            set { _items = value; }
        }
    }
}
