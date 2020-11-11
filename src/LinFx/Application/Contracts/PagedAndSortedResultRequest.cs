using LinFx;
using LinFx.Application;
using LinFx.Application.Contracts;

namespace LinFx.Application.Contracts
{
    /// <summary>
    /// Simply implements <see cref="IPagedAndSortedResultRequest"/>.
    /// </summary>
    public class PagedAndSortedResultRequest : PagedResultRequest, IPagedAndSortedResultRequest
    {
        /// <summary>
        /// 排序
        /// </summary>
        public virtual string Sorting { get; set; }
    }
}
