using System;
using System.Collections.Generic;

namespace LinFx
{
    /// <summary>
    /// Checks
    /// </summary>
    public static class Check
    {
        public static T NotNull<T>(T value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);

            return value;
        }

        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, string paramName)
        {
            if (value.IsNullOrEmpty())
                throw new ArgumentException(paramName + " can not be null or empty!", paramName);

            return value;
        }
    }
}
