using LinFx.Application.Dtos;
using LinFx.Application.Services;
using LinFx.Extensions.Blogging.Domain.Blogs;
using LinFx.Extensions.Blogging.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Blogging
{
    /// <summary>
    /// 博客服务
    /// </summary>
    public class BlogService : ApplicationService
    {
        protected BloggingDbContext _db;

        public BlogService(IServiceProvider context, BloggingDbContext db) : base(context)
        {
            _db = db;
        }

        //public Task<Blog> GetAsync(Guid id)
        //{
        //    return _db.Blogs.FindAsync(id);
        //}

        public async Task<ListResult<Blog>> GetListAsync()
        {
            var items = await _db.Blogs.ToListAsync();
            return new ListResult<Blog>(items);
        }
    }
}
