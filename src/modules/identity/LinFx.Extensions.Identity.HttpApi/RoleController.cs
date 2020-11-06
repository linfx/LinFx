using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.HttpApi
{
    /// <summary>
    /// 角色Api
    /// </summary>
    [ApiController]
    [Route("api/identity/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService<Role> _roleService;

        public RoleController(RoleService<Role> roleService)
        {
            _roleService = roleService;
        }

        ///// <summary>
        ///// 列表
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public virtual Task<PagedResult<IdentityRoleResult>> GetListAsync(IdentityRoleInput input)
        //{
        //    return _roleService.GetList(input);
        //}

        ///// <summary>
        ///// 获取
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("{id}")]
        //public virtual async Task<IdentityRoleResult> GetAsync(string id)
        //{
        //    return await _roleService.GetAsync(id);
        //}

        ///// <summary>
        ///// 创建
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public virtual Task<IdentityUserResult> CreateAsync(IdentityUserInput input)
        //{
        //    return _roleService.CreateAsync(input);
        //}

        ///// <summary>
        ///// 更新
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPut("{id}")]
        //public virtual Task<IdentityRoleResult> UpdateAsync(string id, IdentityRoleUpdateInput input)
        //{
        //    return _roleService.UpdateAsync(id, input);
        //}

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual Task DeleteAsync(string id)
        {
            return _roleService.DeleteAsync(id);
        }
    }
}
