using LinFx.Application.Models;
using LinFx.Module.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.Services
{
    public class RoleService<TRole> where TRole : class
    {
        private readonly RoleManager<TRole> _roleManager;

        public RoleService(RoleManager<TRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return;

            await _roleManager.DeleteAsync(role);
        }

        public async Task<IdentityRoleResult> GetAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return default;

            return role.MapTo<IdentityRoleResult>();
        }

        public Task<IdentityRoleResult> UpdateAsync(string id, IdentityRoleUpdateInput input)
        {
            var item = new IdentityRoleResult();
            return Task.FromResult(item);
        }

        public Task<IdentityUserResult> CreateAsync(IdentityUserInput input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<IdentityRoleResult>> GetList(IdentityRoleInput input)
        {
            throw new NotImplementedException();
        }
    }
}
