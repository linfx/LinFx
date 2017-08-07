using LinFx.Data.Extensions;
using LinFx.Data.Extensions.Mapper;
using Shouldly;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace LinFx.UnitTest.Data.Dapper.Extensions
{
	public class PropertyMapTest
    {
        [Fact]
        public void PropertyMap_Constructor_Sets_Name_And_ColumnName_Property_From_PropertyInfo()
        {
            Expression<Func<Foo, object>> expression = f => f.Bar;
            var pi = ReflectionUtils.GetProperty(expression) as PropertyInfo;
            var pm = new PropertyMap(pi);

            pm.Name.ShouldBe("Bar");
            pm.ColumnName.ShouldBe("Bar");
        }

        [Fact]
        public void PropertyMap_Column_Sets_ColumnName_But_Does_Not_Change_Name()
        {
            Expression<Func<Foo, object>> expression = f => f.Baz;
            var pi = ReflectionUtils.GetProperty(expression) as PropertyInfo;
            var pm = new PropertyMap(pi);
            pm.Column("X");

            pm.Name.ShouldBe("Baz");
            pm.ColumnName.ShouldBe("X");
        }

        [Fact]
        public void PropertyMap_Key_Sets_KeyType()
        {
            Expression<Func<Foo, object>> expression = f => f.Baz;
            var pi = ReflectionUtils.GetProperty(expression) as PropertyInfo;
            var pm = new PropertyMap(pi);
            pm.Column("X").Key(KeyType.Identity);

            pm.Name.ShouldBe("Baz");
            pm.ColumnName.ShouldBe("X");
            pm.KeyType.ShouldBe(KeyType.Identity);
        }
    }
}
