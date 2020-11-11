﻿using LinFx.Extensions.Blogging.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LinFx.Extensions.Blogging.Application.Models;

namespace LinFx.Extensions.Blogging.Application
{
    [Service]
    public class PostService
    {
        protected BloggingDbContext _context;

        public PostService(BloggingDbContext context)
        {
            _context = context;
        }

        public Task<PostWithDetailsDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        //public async Task<PostWithDetails> GetAsync(Guid id)
        //{
        //    var post = await _context.Posts.LoadAsync();

        //    var postDto = ObjectMapper.Map<Post, PostWithDetailsDto>(post);

        //    postDto.Tags = await GetTagsOfPost(postDto.Id);

        //    if (postDto.CreatorId.HasValue)
        //    {
        //        var creatorUser = await UserLookupService.FindByIdAsync(postDto.CreatorId.Value);

        //        postDto.Writer = ObjectMapper.Map<BlogUser, BlogUserDto>(creatorUser);
        //    }

        //    return postDto;
        //}

        //public async Task<ListResult<PostWithDetails>> GetListByBlogIdAndTagName(Guid id, string tagName)
        //{

        //}
    }
}
