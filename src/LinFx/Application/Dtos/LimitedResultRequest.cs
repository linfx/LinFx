using System.ComponentModel.DataAnnotations;

namespace LinFx.Application.Dtos;

public class LimitedResultRequest
{
    /// <summary>
    /// 页大小
    /// </summary>
    [Range(1, int.MaxValue)]
    public virtual int PageSize { get; set; } = 15;
}
