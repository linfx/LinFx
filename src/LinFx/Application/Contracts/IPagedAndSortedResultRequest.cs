using LinFx;
using LinFx.Application;
using LinFx.Application.Contracts;

namespace LinFx.Application.Contracts
{
    /// <summary>
    /// This interface is defined to standardize to request a paged and sorted result.
    /// </summary>
    public interface IPagedAndSortedResultRequest : IPagedResultRequest, ISortedResultRequest
    {
    }
}
