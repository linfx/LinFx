using System.ComponentModel.DataAnnotations;

namespace LinFx.Application.Models
{
    public class LimitedResultRequest
    {
        [Range(1, int.MaxValue)]
        public virtual int Limit { get; set; } = 15;
    }
}