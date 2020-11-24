using LinFx.Application.Models;

namespace LinFx.Extensions.TenantManagement.Application.Models
{
    public class TenantRequest : PagedAndSortedResultRequest
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter { get; set; }
    }
}