using System.ComponentModel.DataAnnotations;

namespace LinFx.Application.Models
{
    public class LimitedResultRequest
    {
        /// <summary>
        /// 页大小
        /// </summary>
        [Range(1, int.MaxValue)]
        public virtual int Limit { get; set; } = 15;
    }
}