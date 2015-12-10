namespace EA.Iws.Core.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether a sequence contains unique elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsUnique<T>(this IEnumerable<T> values)
        {
            var set = new HashSet<T>();

            return values.All(item => set.Add(item));
        }
    }
}