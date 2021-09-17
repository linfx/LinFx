using LinFx.Application.Dtos;

namespace LinFx.Extensions.TenantManagement
{
    public class TenantRequest : PagedAndSortedResultRequest
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter { get; set; }
    }
}