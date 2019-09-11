using System;
using Xunit;

namespace LinFx.Test.ObjectMapping
{
    public class ObjectMappingTest
    {
        [Fact]
        public void MapToTests()
        {
            var person = new Person()
            {
                Name = "Leo",
                Age = 200,
            };

            var dto = person.MapTo<PersonDto>(person);

            Assert.Equal(person.Name, dto.Name);
            Assert.Equal(person.Age, dto.Age);
        }
    }
}
