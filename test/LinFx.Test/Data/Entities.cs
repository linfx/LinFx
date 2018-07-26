using LinFx.Domain.Entities;
using LinFx.Extensions.DapperExtensions.Mapper;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Test.Data.Dapper
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Key]
        public string Name { get; set; }
    }

    public class Post
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		//public User Owner { get; set; }
	}

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
    }

    public class Author : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    //public sealed class PostMap : ClassMapper<Post>
    //{
    //	public PostMap()
    //	{
    //		Map(p => p.Id).Key(KeyType.Identity);
    //		AutoMap();
    //	}
    //}

    public sealed class UseMap : ClassMapper<User>
    {
        public UseMap()
        {
            Table("user");
            Map(x => x.Id).Key(KeyType.NotAKey);
            Map(x => x.Name).Key(KeyType.NotAKey);
            AutoMap();
        }
    }

    public sealed class AuthorMap : ClassMapper<Author>
    {
        public AuthorMap()
        {
            Table("t_author");
            Map(x => x.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
