using LinFx.Unity;
using System.Collections.Generic;
using Xunit;
using System;

namespace LinFx.UnitTest.Unity
{
 public	class Test
    {
		[Fact]
		public void Test1()
		{
		     UnityManager.Instance.Register<IFoo, Foo>();
			 var t = UnityManager.Instance.Resolve<IFoo>();
		}
	}

	public interface IFoo
	{
		string Name { get; set; }
	}

	public class Foo : IFoo
	{
		public string Name { get; set; }
	}
}
