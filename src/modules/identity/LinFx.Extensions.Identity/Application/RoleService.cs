using LinFx.Application.Models;
using LinFx.Extensions.Identity.Application.Models;
using LinFx.Extensions.Identity.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.Application
{
    /// <summary>
    /// 角色服务
    /// </summary>
    /// <typeparam name="TRole"></typeparam>
    public class RoleService<TRole> where TRole : class, new()
    {
        private readonly RoleManager<TRole> _roleManager;

        public RoleService(RoleManager<TRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RoleResult> GetAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return default;

            return role.MapTo<RoleResult>();
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResult<RoleResult>> GetListAsync(PagedAndSortedResultRequest input)
        {
            return _roleManager.Roles
                .Select(p => p.MapTo<RoleResult>())
                .ToPageResultAsync(input);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Roles.Create)]
        public async Task<RoleResult> CreateAsync(RoleCreateInput input)
        {
            var role = new TRole();

            input.MapTo(role);
            await _roleManager.CreateAsync(role);

            return role.MapTo<RoleResult>();
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Roles.Update)]
        public async Task<RoleResult> UpdateAsync(string id, RoleUpdateInput input)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return default;

            input.MapTo(role);
            await _roleManager.UpdateAsync(role);

            return role.MapTo<RoleResult>();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Roles.Delete)]
        public async Task DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return;

            await _roleManager.DeleteAsync(role);
        }
    }
}
