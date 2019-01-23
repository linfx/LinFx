using LinFx.Application.Abstractions;

namespace LinFx.Application.Models
{
    /// <summary>
    /// Simply implements <see cref="IPagedAndSortedResultRequest"/>.
    /// </summary>
    public class PagedAndSortedResultRequestDto : PagedResultRequest, IPagedAndSortedResultRequest
    {
        public virtual string Sorting { get; set; }
    }
}
