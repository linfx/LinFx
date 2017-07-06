using LinFx.Configuration;
using LinFx.Unity;
using LinFx.Logging;

namespace LinFx.Modules
{
	public abstract class Module
	{
		/// <summary>
		/// Gets a reference to the IOC manager.
		/// </summary>
		protected internal IUnityManager UnityManager { get; internal set; }
		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		protected ILogger Logger { get; set; }
		/// <summary>
		/// Gets a reference to the ABP configuration.
		/// </summary>
		protected internal IStartupConfiguration Configuration { get; internal set; }
		/// <summary>
		/// This is the first event called on application startup. 
		/// Codes can be placed here to run before dependency injection registrations.
		/// </summary>
		public virtual void PreInitialize() { }
		/// <summary>
		/// This method is used to register dependencies for this module.
		/// </summary>
		public virtual void Initialize() { }
		/// <summary>
		/// This method is called lastly on application startup.
		/// </summary>
		public virtual void PostInitialize() { }
		/// <summary>
		/// This method is called when the application is being shutdown.
		/// </summary>
		public virtual void Shutdown() { }
	}
}
