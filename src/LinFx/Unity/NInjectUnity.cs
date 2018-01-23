//using Ninject;

//namespace LinFx.Unity
//{
//	public class NInjectUnity : IUnity
//	{
//		IKernel kernel = new StandardKernel();

//		public T Resolve<T>()
//		{
//			return kernel.Get<T>();
//		}

//		void IRegistrar.Register<TType, TImpl>()
//		{
//			kernel.Bind<TType>().To<TImpl>();
//		}
//	}
//}