using System.ComponentModel.DataAnnotations;

namespace LinFx.Application.Models
{
    /// <summary>
    /// Simply implements <see cref="IPagedResultRequest"/>.
    /// </summary>
    public class PagedResultRequest : LimitedResultRequest, IPagedResultRequest
    {
        /// <summary>
        /// 当前页
        /// </summary>
        [Range(1, int.MaxValue)]
        public virtual int Page { get; set; } = 1;
    }
}