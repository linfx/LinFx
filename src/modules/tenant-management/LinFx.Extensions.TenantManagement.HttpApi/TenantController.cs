using LinFx.Application.Models;
using LinFx.Module.TenantManagement.Services;
using LinFx.Module.TenantManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinFx.Module.TenantManagement.HttpApi
{
    /// <summary>
    /// 租户Api
    /// </summary>
    [ApiController]
    [Route("api/multi-tenancy/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _service;

        public TenantController(ITenantService service)
        {
            _service = service;
        }

        /// <summary>
        /// 租户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual Task<PagedResult<TenantResult>> GetListAsync(TenantInput input)
        {
            return _service.GetListAsync(input);
        }

        /// <summary>
        /// 获取租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual Task<TenantResult> GetAsync(string id)
        {
            return _service.GetAsync(id);
        }

        /// <summary>
        /// 创建租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual Task<TenantResult> CreateAsync(TenantCreateInput input)
        {
            return _service.CreateAsync(input);
        }

        /// <summary>
        /// 更新租户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual Task<TenantResult> UpdateAsync(string id, TenantUpdateInput input)
        {
            return _service.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual Task DeleteAsync(string id)
        {
            return _service.DeleteAsync(id);
        }
    }
}