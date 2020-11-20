using LinFx.Application.Models;
using LinFx.Extensions.TenantManagement.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinFx.Extensions.TenantManagement.HttpApi
{
    /// <summary>
    /// 租户Api接口
    /// </summary>
    [ApiController]
    [Route("api/multi-tenancy/tenants")]
    public class TenantController : ControllerBase
    {
        protected ITenantService TenantService;

        public TenantController(ITenantService service)
        {
            TenantService = service;
        }

        /// <summary>
        /// 租户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual Task<PagedResult<TenantDto>> GetListAsync([FromQuery] TenantInput input)
        {
            return TenantService.GetListAsync(input);
        }

        /// <summary>
        /// 获取租户
        /// </summary>
        /// <param name="id">租户Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual Task<TenantDto> GetAsync(string id)
        {
            return TenantService.GetAsync(id);
        }

        /// <summary>
        /// 创建租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual Task<TenantDto> CreateAsync(TenantEditInput input)
        {
            return TenantService.CreateAsync(input);
        }

        /// <summary>
        /// 更新租户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual Task<TenantDto> UpdateAsync(string id, TenantEditInput input)
        {
            return TenantService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual Task DeleteAsync(string id)
        {
            return TenantService.DeleteAsync(id);
        }
    }
}