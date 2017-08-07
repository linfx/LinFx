using LinFx.Data.Extensions;
using Shouldly;
using System;
using System.Linq.Expressions;
using Xunit;

namespace LinFx.UnitTest.Data.Dapper.Extensions
{
	public class ReflectionUtilsTest
    {
        [Fact]
        public void GetProperty_Returns_MemberInfo_For_Correct_Property()
        {
            Expression<Func<Foo, object>> expression = f => f.Bar;
            var m = ReflectionUtils.GetProperty(expression);

            m.Name.ShouldBe("Bar");
        }

        [Fact]
        public void GetObjectValues_Returns_Dictionary_With_Property_Value_Pairs()
        {
            var f = new Foo { Bar = 3, Baz = "Yum" };
            var dictionary = ReflectionUtils.GetObjectValues(f);

            dictionary["Bar"].ShouldBe(3);
            dictionary["Baz"].ShouldBe("Yum");
        }

        [Fact]
        public void GetObjectValues_Returns_Empty_Dictionary_When_Null_Object_Provided()
        {
            var dictionary = ReflectionUtils.GetObjectValues(null);

            dictionary.Count.ShouldBe(0);
        }
    }

    public class Foo
    {
        public int Bar { get; set; }
        public string Baz { get; set; }
    }
}
