namespace LinFx.Dependency
{
	public	interface IIocResolver
    {
		/// <summary>
		/// Gets an object from IOC container.
		/// Returning object must be Released (see <see cref="Release"/>) after usage.
		/// </summary> 
		/// <typeparam name="T">Type of the object to get</typeparam>
		/// <returns>The object instance</returns>
		T Resolve<T>();
	}
}