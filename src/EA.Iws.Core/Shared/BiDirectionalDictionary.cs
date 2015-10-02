namespace EA.Iws.Core.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A dictionary which allows lookup by both value and key.
    /// </summary>
    /// <typeparam name="T1">The first type of key.</typeparam>
    /// <typeparam name="T2">The second type of key.</typeparam>
    public class BidirectionalDictionary<T1, T2>
    {
        private readonly IDictionary<T1, T2> forwardLookup;
        private readonly IDictionary<T2, T1> reverseLookup;

        public BidirectionalDictionary(Dictionary<T1, T2> dictionary)
        {
            if (dictionary.Values.Distinct().Count() != dictionary.Values.Count)
            {
                throw new ArgumentException("This lookup would contain a duplicate key");
            }

            forwardLookup = dictionary;
            reverseLookup = dictionary.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        public T1 this[T2 index]
        {
            get { return reverseLookup[index]; }
        }

        public T2 this[T1 index]
        {
            get { return forwardLookup[index]; }
        }

        public bool TryGetByFirst(T1 first, out T2 second)
        {
            return forwardLookup.TryGetValue(first, out second);
        }

        public bool TryGetBySecond(T2 second, out T1 first)
        {
            return reverseLookup.TryGetValue(second, out first);
        }

        public bool ContainsKey(T1 key)
        {
            return forwardLookup.ContainsKey(key);
        }

        public bool ContainsKey(T2 key)
        {
            return reverseLookup.ContainsKey(key);
        }
    }
}
