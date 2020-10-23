using LinFx.Extensions.Blogging.Application;
using LinFx.Extensions.Blogging.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Blogging.HttpApi
{
    [Route("api/blogging/posts")]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;

        public PostsController(PostService postService)
        {
            _postService = postService;
        }

        [HttpGet("{id}")]
        public Task<PostWithDetailsDto> GetAsync(Guid id)
        {
            return _postService.GetAsync(id);
        }
    }
}
