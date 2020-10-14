using LinFx.Extensions.Blogging.Domain.Models;

namespace LinFx.Extensions.Blogging.Application.Services
{
    public class PostWithDetails : Post
    {
        public int CommentCount { get; set; }
    }
}