using LinFx.Application.Contracts;

namespace LinFx.Extensions.TenantManagement
{
    public class TenantInput : PagedAndSortedResultRequest
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter { get; set; }
    }
}