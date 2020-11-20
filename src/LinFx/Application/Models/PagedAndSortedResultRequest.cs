namespace LinFx.Application.Models
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
