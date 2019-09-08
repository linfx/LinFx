namespace LinFx.Application.Models
{
    public class FilterPagedAndSortedResultRequest : PagedAndSortedResultRequest, IFilterRequest
    {
        /// <summary>
        ///过滤
        /// </summary>
        public string Filter { get; set; }
    }
}
