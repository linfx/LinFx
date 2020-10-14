using LinFx.Extensions.Blogging.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.Blogging.EntityFrameworkCore
{
    public class BloggingDbContext : Extensions.EntityFrameworkCore.DbContext
    {
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<BlogUser> Users { get; set; }

        /// <summary>
        /// 博客
        /// </summary>
        public DbSet<Blog> Blogs { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// 帖子
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        /// <summary>
        /// 评论
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlogUser>(b =>
            {
            });

            modelBuilder.Entity<Blog>(b =>
            {
                b.Property(x => x.Name).IsRequired().HasMaxLength(200).HasColumnName(nameof(Blog.Name));
                b.Property(x => x.ShortName).IsRequired().HasMaxLength(100).HasColumnName(nameof(Blog.ShortName));
                b.Property(x => x.Description).IsRequired(false).HasMaxLength(2000).HasColumnName(nameof(Blog.Description));
            });
        }
    }
}
