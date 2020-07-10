using LinFx.Test.ObjectMapping;
using System;
using System.Collections.Generic;
using Xunit;

namespace LinFx.Test.Extensions.ObjectMapping
{
    public class ObjectMappingTest
    {
        [Fact]
        public void MapTo_Tests()
        {
            var person = new Person
            {
                Name = "Leo",
                Age = 200,
            };

            var dto = person.MapTo<PersonDto>();
            Assert.Equal(person.Name, dto.Name);
            Assert.Equal(person.Age, dto.Age);
        }

        [Fact]
        public void AsMap_Tests()
        {
            var person = new Person
            {
                Name = "Leo",
                Age = 200,
                Teacher = new Teacher
                {
                    Id = 1,
                    Name = "mmml"
                }
            };

            //var dto = person.AsMap()
            //     .ConvertUsing<Teacher, string>(t => t.Name)
            //     .To<PersonDto>();

            //Assert.Equal(person.Name, dto.Name);
            //Assert.Equal(person.Age, dto.Age);
        }

        [Fact]
        public void MapTo_Array_Tests()
        {
            var persions = new List<Person>
            {
                new Person { Name = "Lio1", Age = 11 },
                new Person { Name = "Lio1", Age = 12 },
                new Person { Name = "Lio1", Age = 13 },
            }.ToArray();

            var dtos = persions.MapTo<PersonDto[]>();
            Assert.Equal(persions.Length, dtos.Length);

            var dtos2 = new PersonDto[persions.Length];
            persions.MapTo(dtos2);
            Assert.Equal(persions.Length, dtos2.Length);
        }

        [Fact]
        public void MapTo_List_Tests()
        {
            var persions = new List<Person>
            {
                new Person { Name = "Lio1", Age = 11 },
                new Person { Name = "Lio1", Age = 12 },
                new Person { Name = "Lio1", Age = 13 },
            };

            var dtos = persions.MapTo<List<PersonDto>>();
            Assert.Equal(persions.Count, dtos.Count);
        }


    }
}
