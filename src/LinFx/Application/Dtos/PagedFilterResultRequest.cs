using System.Linq.Expressions;

namespace LinFx.Application.Dtos;

/// <summary>
/// Simply implements <see cref="IPagedResultRequest"/>.
/// </summary>
public class PagedFilterResultRequest<T> : PagedResultRequest
{
    /// <summary>
    /// 条件表达式
    /// </summary>
    public Expression<Func<T, bool>>? Expression { get; set; }
}