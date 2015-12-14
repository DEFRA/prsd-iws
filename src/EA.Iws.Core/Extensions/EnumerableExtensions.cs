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

        /// <summary>
        /// Determines whether a sequence of integers is consecutive
        /// 
        /// http://stackoverflow.com/a/13359693
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsConsecutive(this IEnumerable<int> values)
        {
            return !values.Select((i, j) => i - j).Distinct().Skip(1).Any();
        }
    }
}