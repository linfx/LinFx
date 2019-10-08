using Elasticsearch.Net;
using LinFx.Utils;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.Test.Elasticsearch
{
    public class DocumentTests
    {
        private readonly IElasticClient _client;

        public DocumentTests()
        {
            var services = new ServiceCollection();
            services
                .AddLinFx()
                .AddElasticsearch(options =>
                {
                    options.Host = "http://10.0.1.222:9200";
                    options.Index = "my-index";
                });
            var container = services.BuildServiceProvider();
            _client = container.GetService<IElasticClient>();
        }

        [Fact]
        public void Get()
        {
            var response = _client.Get<User>(7221992037892096);

            Assert.True(response.ApiCall.Success);
        }

        [Fact]
        public async Task IndexAsync()
        {
            //var post = new Post
            //{
            //    Title = "Hello World"
            //};
            //var request = new IndexRequest<Post>(post);
            //var response = _client.Index(post, idx => idx.Index("post"));
 
            var collection = new string[] { "林松斌", "成龙", "陈国伟", "唐峰" };
            foreach (var item in collection)
            {
                var user = new User
                {
                    Id = IDUtils.NewId(),
                    Name = item
                };

                var request = new IndexRequest<User>(user);
                var response = await _client.IndexAsync(user, idx => idx.Index(nameof(User).ToLower()));
                Assert.True(response.ApiCall.Success);
            }
        }

        [Fact]
        public void Create()
        {
            var user = new User
            {
                Id = IDUtils.NewId(),
                Name = DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff")
            };

            var request = new CreateRequest<User>(user);

            var response = _client.Create(request);

            Assert.True(response.ApiCall.Success);
        }

        [Fact]
        public void CreateDocument()
        {
            var user = new User
            {
                Id = IDUtils.NewId(),
                Name = DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff")
            };

            var request = new CreateRequest<User>(user.Id);

            var response = _client.CreateDocument(user);

            Assert.True(response.ApiCall.Success);
        }

        /// <summary>
        /// 搜索
        /// </summary>
        [Fact]
        public async Task SearchAsync()
        {
            var user = new User
            {
                Id = IDUtils.NewId(),
            };


            var request = new SearchRequest<User>();
            var response = await _client.SearchAsync<User>();

            Assert.True(response.ApiCall.Success);
        }
    }
}
