using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace LinFx.Modules
{
    public class ModuleLoader
    {
        public ModuleInfo[] LoadModules(IServiceCollection services, Type startupModuleType)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(startupModuleType, nameof(startupModuleType));

            var modules = GetModuleInfos(services, startupModuleType);
            ConfigureServices(services, modules);
            return modules.ToArray();
        }

        private List<ModuleInfo> GetModuleInfos(IServiceCollection services, Type startupModuleType)
        {
            var modules = new List<ModuleInfo>();
            FillModules(services, startupModuleType, modules);
            return modules;
        }

        protected virtual void FillModules(IServiceCollection services, Type startupModuleType, List<ModuleInfo> modules)
        {
            // All modules starting from the startup module
            foreach (var moduleType in ModuleHelper.FindAllModuleTypes(startupModuleType))
            {
                modules.Add(CreateModuleInfo(services, moduleType));
            }
        }

        protected virtual ModuleInfo CreateModuleInfo(IServiceCollection services, Type moduleType)
        {
            return new ModuleInfo(moduleType, CreateAndRegisterModule(services, moduleType));
        }

        protected virtual IModuleInitializer CreateAndRegisterModule(IServiceCollection services, Type moduleType)
        {
            var module = (IModuleInitializer)Activator.CreateInstance(moduleType);
            services.AddSingleton(moduleType, module);
            return module;
        }

        protected virtual void ConfigureServices(IServiceCollection services, List<ModuleInfo> modules)
        {
            // ConfigureServices
            foreach (var module in modules)
            {
                //if (module.Instance is ModuleInfo abpModule)
                //{
                //    if (!abpModule.SkipAutoServiceRegistration)
                //    {
                //        services.AddAssembly(module.Type.Assembly);
                //    }
                //}

                module.Instance.ConfigureServices(services);
            }
        }
    }
}
