using System;
using AutoMapper;
using LinFx.Extensions;

namespace LinFx.ObjectMapping
{
	public abstract class AutoMapAttributeBase : Attribute
	{
		public Type[] TargetTypes { get; private set; }

		protected AutoMapAttributeBase(params Type[] targetTypes)
		{
			TargetTypes = targetTypes;
		}

		public abstract void CreateMap(IMapperConfigurationExpression configuration, Type type);
	}

	public class AutoMapAttribute : AutoMapAttributeBase
    {
        public AutoMapAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {

        }

        public override void CreateMap(IMapperConfigurationExpression configuration, Type type)
        {
            if (TargetTypes.IsNullOrEmpty())
                return;

            foreach (var targetType in TargetTypes)
            {
                configuration.CreateMap(type, targetType, MemberList.Source);
                configuration.CreateMap(targetType, type, MemberList.Destination);
            }
        }
    }

	public class AutoMapFromAttribute : AutoMapAttributeBase
	{
		public MemberList MemberList { get; set; } = MemberList.Destination;

		public AutoMapFromAttribute(params Type[] targetTypes)
			: base(targetTypes)
		{

		}

		public AutoMapFromAttribute(MemberList memberList, params Type[] targetTypes)
			: this(targetTypes)
		{
			MemberList = memberList;
		}

		public override void CreateMap(IMapperConfigurationExpression configuration, Type type)
		{
			if(TargetTypes.IsNullOrEmpty())
			{
				return;
			}

			foreach(var targetType in TargetTypes)
			{
				configuration.CreateMap(targetType, type, MemberList);
			}
		}
	}
}