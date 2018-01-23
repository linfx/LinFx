namespace LinFx.Unity
{
	public interface IRegistrar
	{
		/// <summary>
		/// Registers a type with it's implementation.
		/// </summary>
		/// <typeparam name="TType">Registering type</typeparam>
		/// <typeparam name="TImpl">The type that implements <see cref="TType"/></typeparam>
		/// <param name="lifeStyle">Lifestyle of the objects of this type</param>
		void Register<TType, TImpl>()
			where TType : class
			where TImpl : class, TType;
	}

	public interface IResolve
	{
		/// <summary>
		/// Gets an object from IOC container.
		/// Returning object must be Released (see <see cref="Release"/>) after usage.
		/// </summary> 
		/// <typeparam name="T">Type of the object to get</typeparam>
		/// <returns>The object instance</returns>
		T Resolve<T>();
	}

	public interface IUnity : IRegistrar, IResolve
	{
	}

	public interface IUnityManager : IUnity
	{
	}

	public class UnityManager : IUnityManager
    {
        /// <summary>
        /// The Singleton instance.
        /// </summary>
        public static UnityManager Instance { get; } = new UnityManager();
		/// <summary>
		/// Container
		/// </summary>
		//IUnity kernel = new NInjectUnity();

		public T Resolve<T>()
		{
            //return kernel.Resolve<T>();
            return default(T);
		}

	    public void Register<TType, TImpl>()
			where TType : class
			where TImpl : class, TType
		{
            //kernel.Register<TType, TImpl>();
        }
	}
}
