using System;
using System.Reflection;

namespace LinFx.Utils
{
    /// <summary>
    /// Some simple type-checking methods used internally.
    /// </summary>
    public static class TypeUtils
    {
        public static bool IsFunc(object obj)
        {
            if (obj == null)
                return false;

            var type = obj.GetType();
            if (!type.GetTypeInfo().IsGenericType)
                return false;

            return type.GetGenericTypeDefinition() == typeof(Func<>);
        }

        public static bool IsFunc<TReturn>(object obj)
        {
            return obj != null && obj.GetType() == typeof(Func<TReturn>);
        }

        public static bool IsPrimitiveExtendedIncludingNullable(Type type, bool includeEnums = false)
        {
            if (IsPrimitiveExtended(type, includeEnums))
                return true;

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return IsPrimitiveExtended(type.GenericTypeArguments[0], includeEnums);

            return false;
        }

        private static bool IsPrimitiveExtended(Type type, bool includeEnums)
        {
            if (type.GetTypeInfo().IsPrimitive)
                return true;

            if (includeEnums && type.GetTypeInfo().IsEnum)
                return true;

            return type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }
    }

    public static class TypeExtensions
    {
        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

        /// <summary>
        /// Determines whether an instance of this type can be assigned to
        /// an instance of the <typeparamref name="TTarget"></typeparamref>.
        ///
        /// Internally uses <see cref="Type.IsAssignableFrom"/>.
        /// </summary>
        /// <typeparam name="TTarget">Target type</typeparam> (as reverse).
        public static bool IsAssignableTo<TTarget>(this Type type)
        {
            return type.IsAssignableTo(typeof(TTarget));
        }

        /// <summary>
        /// Determines whether an instance of this type can be assigned to
        /// an instance of the <paramref name="targetType"></paramref>.
        ///
        /// Internally uses <see cref="Type.IsAssignableFrom"/> (as reverse).
        /// </summary>
        /// <param name="type">this type</param>
        /// <param name="targetType">Target type</param>
        public static bool IsAssignableTo(this Type type, Type targetType)
        {
            return targetType.IsAssignableFrom(type);
        }
    }
}