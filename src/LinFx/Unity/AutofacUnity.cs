using Autofac;

namespace LinFx.Unity
{
	public class AutofacUnity : IUnity
	{
		IContainer container;
		ContainerBuilder builder = new ContainerBuilder();

		public AutofacUnity()
		{
			//container = builder.Build();
		}

		public T Resolve<T>()
		{
			container = builder.Build();
			return container.Resolve<T>();
		}

		void IRegistrar.Register<TType, TImpl>()
		{
			builder.RegisterType<TImpl>().As<TType>();
		}
	}
}
