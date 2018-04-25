using LinFx.Extensions;
using System;
using System.Collections.Generic;

namespace LinFx
{
    /// <summary>
    /// 检查
    /// </summary>
    public static class Check
    {
        public static T NotNull<T>(T value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
            return value;
        }

        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, string parameterName)
        {
            if (value.IsNullOrEmpty())
                throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
            return value;
        }
    }
}
