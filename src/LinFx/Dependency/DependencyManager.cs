using System;

namespace LinFx.Dependency
{
    public class DependencyManager
    {
        /// <summary>
        /// The Singleton instance.
        /// </summary>
        public static DependencyManager Instance { get; } = new DependencyManager();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TImplementer"></typeparam>
        public static void Register<T>()
        {
            //builder.RegisterType<T>().AsSelf().PropertiesAutowired();
            throw new NotImplementedException();
        }

        public T Resolve<T>(Type type)
        {
            //return (T)IocContainer.Resolve(type);
            throw new NotImplementedException();
        }
    }
}
