using LinFx.Application.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Application.Models
{
    /// <summary>
    /// Simply implements <see cref="IPagedResultRequest"/>.
    /// </summary>
    public class PagedResultRequest : LimitedResultRequest, IPagedResultRequest
    {
        [Range(1, int.MaxValue)]
        public virtual int Page { get; set; } = 1;
    }
}