using System;
using System.Collections.Generic;
using AutoMapper;
using System.Reflection;

namespace LinFx.ObjectMapping
{
	public interface IAutoMapperConfiguration
	{
		List<Action<IMapperConfigurationExpression>> Configurators { get; }
		/// <summary>
		/// Use static <see cref="Mapper.Instance"/>.
		/// Default: true.
		/// </summary>
		bool UseStaticMapper { get; set; }
	}

	public class AutoMapperConfiguration : IAutoMapperConfiguration
    {
        public List<Action<IMapperConfigurationExpression>> Configurators { get; }

        public bool UseStaticMapper { get; set; }

        public AutoMapperConfiguration()
        {
            UseStaticMapper = true;
            Configurators = new List<Action<IMapperConfigurationExpression>>();
        }
    }

	internal static class AutoMapperConfigurationExtensions
	{
		public static void CreateAutoAttributeMaps(this IMapperConfigurationExpression configuration, Type type)
		{
			foreach(var autoMapAttribute in type.GetTypeInfo().GetCustomAttributes<AutoMapAttributeBase>())
			{
				autoMapAttribute.CreateMap(configuration, type);
			}
		}
	}

	//public static class AbpAutoMapperConfigurationExtensions
	//{
	//	/// <summary>
	//	/// Used to configure LinFx.ObjectMapping module.
	//	/// </summary>
	//	public static IAbpAutoMapperConfiguration AutoMapper(this IModuleConfigurations configurations)
	//	{
	//		return configurations.AbpConfiguration.Get<IAbpAutoMapperConfiguration>();
	//	}
	//}
}