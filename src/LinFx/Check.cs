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

        /// <summary>
        /// Adds an item to the collection if it's not already in the collection.
        /// </summary>
        /// <param name="source">Collection</param>
        /// <param name="item">Item to check and add</param>
        /// <typeparam name="T">Type of the items in the collection</typeparam>
        /// <returns>Returns True if added, returns False if not.</returns>
        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source.Contains(item))
                return false;

            source.Add(item);

            return true;
        }
    }
}
