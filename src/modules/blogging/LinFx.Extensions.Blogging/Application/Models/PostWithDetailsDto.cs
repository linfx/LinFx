using LinFx.Extensions.Blogging.Domain.Posts;

namespace LinFx.Extensions.Blogging.Application.Models
{
    public class PostWithDetailsDto : Post
    {
        public int CommentCount { get; set; }
    }
}