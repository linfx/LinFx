using System.ComponentModel.DataAnnotations;

namespace LinFx.Application.Models
{
    public class LimitedResultRequest
    {
        [Range(1, int.MaxValue)]
        public virtual int PageSize { get; set; } = 10;
    }
}