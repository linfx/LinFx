using System;
using LinFx.Data.Dapper.Extensions.Mapper;
using LinFx.Domain.Entities;

namespace LinFx.UnitTest.Data.Dapper
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserEx
    {
        public int Id { get; set; }
        public string NameEx { get; set; }
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
