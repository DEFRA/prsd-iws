namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class RangeExtensions
    {
        /// <summary>
        /// Convert a list of numbers to a number range string.
        /// e.g. 1,2,3,5,6,7,10 becomes "1-3, 5-7, 10"
        /// </summary>
        /// <param name="list">the list of integers</param>
        /// <returns>a number range string</returns>
        public static string ToRangeString(this IEnumerable<int> list)
        {
            return string.Join(", ", list.OrderBy(x => x).ToPossiblyDegenerateRanges().Select(PrettyRange));
        }

        /// <summary>
        /// e.g. 1,3,5,6,7,8,9,10,12
        /// becomes
        /// (1,1),(3,3),(5,10),(12,12)
        /// </summary>
        private static IEnumerable<Tuple<int, int>> ToPossiblyDegenerateRanges(this IEnumerable<int> list)
        {
            Tuple<int, int> currentRange = null;
            foreach (var num in list)
            {
                if (currentRange == null)
                {
                    currentRange = Tuple.Create(num, num);
                }
                else if (currentRange.Item2 == num - 1)
                {
                    currentRange = Tuple.Create(currentRange.Item1, num);
                }
                else
                {
                    yield return currentRange;
                    currentRange = Tuple.Create(num, num);
                }
            }
            if (currentRange != null)
            {
                yield return currentRange;
            }
        }

        /// <summary>
        /// e.g. (1,1) becomes "1"
        /// (1,3) becomes "1-3"
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        private static string PrettyRange(Tuple<int, int> range)
        {
            if (range.Item1 == range.Item2)
            {
                return range.Item1.ToString();
            }
            return string.Format("{0}-{1}", range.Item1, range.Item2);
        }
    }
}