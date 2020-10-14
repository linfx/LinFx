using LinFx.Application.Models;
using LinFx.Extensions.Identity.Application.Models;
using LinFx.Extensions.Identity.EntityFrameworkCore;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.Application
{
    public class UserService
    {
        private readonly IdentityDbContext _context;

        public UserService(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<UserResult>> GetListAsync(UserInput input)
        {
            var count = await _context.Users.CountAsync();
            var items = await _context.Users
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), u =>
                        u.UserName.Contains(input.Filter) ||
                        u.Email.Contains(input.Filter))
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            return new PagedResult<UserResult>(count, items.MapTo<UserResult[]>());
        }

        public async Task<UserResult> GetAsync(string id)
        {
            var item = await _context.Users.FindAsync(id);
            if (item == null)
                throw new UserFriendlyException($"对象[{id}]不存在");

            return item.MapTo<UserResult>();
        }

        public Task<UserResult> UpdateAsync(string id, UserUpdateInput input)
        {
            throw new NotImplementedException();
        }

        public Task<UserResult> CreateAsync(UserInput input)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
