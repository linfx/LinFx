using LinFx.Application.Models;
using LinFx.Extensions.Identity.Data;
using LinFx.Module.Identity.ViewModels;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.Services
{
    public class UserService
    {
        private readonly IdentityDbContext _context;

        public UserService(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<IdentityUserResult>> GetListAsync(IdentityUserInput input)
        {
            var count = await _context.Users.CountAsync();
            var items = await _context.Users
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), u =>
                        u.UserName.Contains(input.Filter) ||
                        u.Email.Contains(input.Filter))
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            return new PagedResult<IdentityUserResult>(count, items.MapTo<IdentityUserResult[]>());
        }

        public async Task<IdentityUserResult> GetAsync(string id)
        {
            var item = await _context.Users.FindAsync(id);
            if (item == null)
                throw new UserFriendlyException($"对象[{id}]不存在");

            return item.MapTo<IdentityUserResult>();
        }

        public Task<IdentityUserResult> UpdateAsync(string id, IdentityUserUpdateInput input)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUserResult> CreateAsync(IdentityUserInput input)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
