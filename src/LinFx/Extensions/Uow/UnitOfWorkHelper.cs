﻿using System.Reflection;

namespace LinFx.Extensions.Uow;

public static class UnitOfWorkHelper
{
    public static bool IsUnitOfWorkType(TypeInfo implementationType)
    {
        //Explicitly defined UnitOfWorkAttribute
        if (HasUnitOfWorkAttribute(implementationType) || AnyMethodHasUnitOfWorkAttribute(implementationType))
            return true;

        //Conventional classes
        if (typeof(IUnitOfWorkEnabled).GetTypeInfo().IsAssignableFrom(implementationType))
            return true;

        return false;
    }

    public static bool IsUnitOfWorkMethod(MethodInfo methodInfo, out UnitOfWorkAttribute? unitOfWorkAttribute)
    {
        Check.NotNull(methodInfo, nameof(methodInfo));

        //Method declaration
        var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
        if (attrs.Any())
        {
            unitOfWorkAttribute = attrs.First();
            return !unitOfWorkAttribute.IsDisabled;
        }

        if (methodInfo.DeclaringType != null)
        {
            //Class declaration
            attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Any())
            {
                unitOfWorkAttribute = attrs.First();
                return !unitOfWorkAttribute.IsDisabled;
            }

            //Conventional classes
            if (typeof(IUnitOfWorkEnabled).GetTypeInfo().IsAssignableFrom(methodInfo.DeclaringType))
            {
                unitOfWorkAttribute = null;
                return true;
            }
        }

        unitOfWorkAttribute = null;
        return false;
    }

    /// <summary>
    /// 获取当前方法工作单元特性
    /// </summary>
    /// <param name="methodInfo">方法类型</param>
    /// <returns><see cref="UnitOfWorkAttribute"/></returns>
    public static UnitOfWorkAttribute? GetUnitOfWorkAttributeOrNull(MethodInfo methodInfo)
    {
        var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
        if (attrs.Length > 0)
            return attrs[0];

        attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
        if (attrs.Length > 0)
            return attrs[0];

        return default;
    }

    private static bool AnyMethodHasUnitOfWorkAttribute(TypeInfo implementationType) => implementationType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(HasUnitOfWorkAttribute);

    private static bool HasUnitOfWorkAttribute(MemberInfo methodInfo) => methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
}
