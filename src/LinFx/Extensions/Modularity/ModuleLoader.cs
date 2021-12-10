using LinFx.Extensions.Modularity.PlugIns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinFx.Extensions.Modularity
{
    public class ModuleLoader : IModuleLoader
    {
        public IModuleDescriptor[] LoadModules(
            IServiceCollection services,
            Type startupModuleType,
            PlugInSourceList plugInSources)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(startupModuleType, nameof(startupModuleType));
            Check.NotNull(plugInSources, nameof(plugInSources));

            var modules = GetDescriptors(services, startupModuleType, plugInSources);
            modules = SortByDependency(modules, startupModuleType);
            return modules.ToArray();
        }

        private List<IModuleDescriptor> GetDescriptors(
            IServiceCollection services,
            Type startupModuleType,
            PlugInSourceList plugInSources)
        {
            var modules = new List<ModuleDescriptor>();

            FillModules(modules, services, startupModuleType, plugInSources);
            SetDependencies(modules);

            return modules.Cast<IModuleDescriptor>().ToList();
        }

        protected virtual void FillModules(
            List<ModuleDescriptor> modules,
            IServiceCollection services,
            Type startupModuleType,
            PlugInSourceList plugInSources)
        {
            //var logger = services.GetSingletonInstance<ILoggerFactory>().CreateLogger<ApplicationBase>();
            ILogger logger = null;

            //All modules starting from the startup module
            foreach (var moduleType in ModuleHelper.FindAllModuleTypes(startupModuleType, logger))
            {
                modules.Add(CreateModuleDescriptor(services, moduleType));
            }

            //Plugin modules
            foreach (var moduleType in plugInSources.GetAllModules(logger))
            {
                if (modules.Any(m => m.Type == moduleType))
                    continue;

                modules.Add(CreateModuleDescriptor(services, moduleType, isLoadedAsPlugIn: true));
            }
        }

        protected virtual void SetDependencies(List<ModuleDescriptor> modules)
        {
            foreach (var module in modules)
            {
                SetDependencies(modules, module);
            }
        }

        protected virtual List<IModuleDescriptor> SortByDependency(List<IModuleDescriptor> modules, Type startupModuleType)
        {
            var sortedModules = modules.SortByDependencies(m => m.Dependencies);
            //sortedModules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
            return sortedModules;
        }

        protected virtual ModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType, bool isLoadedAsPlugIn = false)
        {
            return new ModuleDescriptor(moduleType, CreateAndRegisterModule(services, moduleType), isLoadedAsPlugIn);
        }

        protected virtual IModule CreateAndRegisterModule(IServiceCollection services, Type moduleType)
        {
            var module = (IModule)Activator.CreateInstance(moduleType);
            services.AddSingleton(moduleType, module);
            return module;
        }

        protected virtual void SetDependencies(List<ModuleDescriptor> modules, ModuleDescriptor module)
        {
            foreach (var dependedModuleType in ModuleHelper.FindDependedModuleTypes(module.Type))
            {
                var dependedModule = modules.FirstOrDefault(m => m.Type == dependedModuleType);
                if (dependedModule == null)
                {
                    throw new Exception("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + module.Type.AssemblyQualifiedName);
                }

                module.AddDependency(dependedModule);
            }
        }

        [Obsolete]
        public ModuleInfo[] LoadModules(IServiceCollection services, Type startupModuleType)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(startupModuleType, nameof(startupModuleType));

            var modules = GetModuleInfos(services, startupModuleType);
            ConfigureServices(services, modules);
            return modules.ToArray();
        }

        [Obsolete]
        private List<ModuleInfo> GetModuleInfos(IServiceCollection services, Type startupModuleType)
        {
            var modules = new List<ModuleInfo>();
            FillModules(services, startupModuleType, modules);
            return modules;
        }

        [Obsolete]
        protected virtual void FillModules(IServiceCollection services, Type startupModuleType, List<ModuleInfo> modules)
        {
            // All modules starting from the startup module
            foreach (var moduleType in ModuleHelper.FindAllModuleTypes(startupModuleType))
            {
                modules.Add(CreateModuleInfo(services, moduleType));
            }
        }

        [Obsolete]
        protected virtual ModuleInfo CreateModuleInfo(IServiceCollection services, Type moduleType)
        {
            return new ModuleInfo(moduleType, CreateAndRegisterModuleInfo(services, moduleType));
        }

        [Obsolete]
        protected virtual IModuleInitializer CreateAndRegisterModuleInfo(IServiceCollection services, Type moduleType)
        {
            var module = (IModuleInitializer)Activator.CreateInstance(moduleType);
            services.AddSingleton(moduleType, module);
            return module;
        }

        [Obsolete]
        protected virtual void ConfigureServices(IServiceCollection services, List<ModuleInfo> modules)
        {
            // ConfigureServices
            foreach (var module in modules)
            {
                if (module.Instance is ModuleInfo item)
                {
                    // 是否跳过服务的自动注册，默认为 false
                    //if (!abpModule.SkipAutoServiceRegistration)
                    //{
                    //    services.AddAssembly(module.Type.Assembly);
                    //}
                }
                module.Instance.ConfigureServices(services);
            }
        }
    }
}
