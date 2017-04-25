using System;

namespace LinFx
{
    public static class Check
    {
        public static T NotNull<T>(T value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
            return value;
        }
    }
}
