using System;
using System.Reflection;
using AutoMapper;

namespace LinFx.ObjectMapping
{
	//[DependsOn(typeof(AbpKernelModule))]
	public class AutoMapperModule : Modules.Module
    {
        //private readonly ITypeFinder _typeFinder;
        private static volatile bool _createdMappingsBefore;
        private static readonly object SyncObj = new object();
        
        //public AutoMapperModule(ITypeFinder typeFinder)
        //{
        //    _typeFinder = typeFinder;
        //}

        public override void PreInitialize()
        {
            UnityManager.Register<IAutoMapperConfiguration, AutoMapperConfiguration>();
            //Configuration.ReplaceService<ObjectMapping.IObjectMapper, AutoMapperObjectMapper>();
            //Configuration.Modules.AbpAutoMapper().Configurators.Add(CreateCoreMappings);
        }

        public override void PostInitialize()
        {
            CreateMappings();
        }

        private void CreateMappings()
        {
            lock (SyncObj)
            {
                Action<IMapperConfigurationExpression> configurer = configuration =>
                {
                    //FindAndAutoMapTypes(configuration);
                    //foreach (var configurator in Configuration.Modules.AbpAutoMapper().Configurators)
                    //{
                    //    configurator(configuration);
                    //}
                };

                //if (Configuration.Modules.AbpAutoMapper().UseStaticMapper)
                //{
                //    //We should prevent duplicate mapping in an application, since Mapper is static.
                //    if (!_createdMappingsBefore)
                //    {
                //        Mapper.Initialize(configurer);
                //        _createdMappingsBefore = true;
                //    }

                //    UnityManager.IocContainer.Register(
                //        Component.For<IMapper>().Instance(Mapper.Instance).LifestyleSingleton()
                //    );
                //}
                //else
                //{
                //    var config = new MapperConfiguration(configurer);
                //    UnityManager.IocContainer.Register(
                //        Component.For<IMapper>().Instance(config.CreateMapper()).LifestyleSingleton()
                //    );
                //}
            }
        }

        //private void FindAndAutoMapTypes(IMapperConfigurationExpression configuration)
        //{
        //    var types = _typeFinder.Find(type =>
        //        {
        //            var typeInfo = type.GetTypeInfo();
        //            return typeInfo.IsDefined(typeof(AutoMapAttribute)) ||
        //                   typeInfo.IsDefined(typeof(AutoMapFromAttribute)) ||
        //                   typeInfo.IsDefined(typeof(AutoMapToAttribute));
        //        }
        //    );

        //    Logger.DebugFormat("Found {0} classes define auto mapping attributes", types.Length);

        //    foreach (var type in types)
        //    {
        //        Logger.Debug(type.FullName);
        //        configuration.CreateAutoAttributeMaps(type);
        //    }
        //}

        //private void CreateCoreMappings(IMapperConfigurationExpression configuration)
        //{
        //    var localizationContext = UnityManager.Resolve<ILocalizationContext>();

        //    configuration.CreateMap<ILocalizableString, string>().ConvertUsing(ls => ls?.Localize(localizationContext));
        //    configuration.CreateMap<LocalizableString, string>().ConvertUsing(ls => ls == null ? null : localizationContext.LocalizationManager.GetString(ls));
        //}
    }
}
