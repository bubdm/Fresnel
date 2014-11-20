using System;
using System.Collections;
using System.Collections.Generic;


namespace Envivo.Fresnel.Utils
{
    /// <summary>
    /// Helper methods for Lists, and Dictionaries
    /// </summary>
    public static class CollectionExtensions
    {
       
        public static bool DoesNotContain<T>(this IEnumerable<T> items, T item)
        {
            return System.Linq.Enumerable.Contains(items, item) == false;
        }

        /// <summary>
        /// Returns TRUE if the item exists in the dictionary. This can perform MUCH faster than ContainsKey() or ContainsValue()
        /// </summary>
        public static bool Contains<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key)
        {
            TValue dummy;
            return items.TryGetValue(key, out dummy);
        }

        /// <summary>
        /// Returns TRUE if the item does NOT exist in the dictionary. This performs MUCH faster than ContainsKey() or ContainsValue()
        /// </summary>
        public static bool DoesNotContain<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key)
        {
            return Contains(items, key) == false;
        }

        /// <summary>
        /// Like TryGetValue, but doesn't use 'out' parameters
        /// </summary>
        public static TValue TryGetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key)
        {
            TValue val;
            return items.TryGetValue(key, out val) ? val : default(TValue);
        }

        /// <summary>
        /// Like TryGetValue, but doesn't use 'out' parameters
        /// </summary>
        public static TValue TryGetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key, TValue defaultValue)
        {
            TValue val;
            return items.TryGetValue(key, out val) ? val : defaultValue;
        }

        /// <summary>
        /// Null safe version of IList.Clear()
        /// </summary>
        public static void ClearSafely<T>(this IList<T> list)
        {
            if (list != null)
            {
                list.Clear();
            }
        }

        /// <summary>
        /// Null safe version of IList.Clear()
        /// </summary>
        public static void ClearSafely<T>(this ICollection<T> collection)
        {
            if (collection != null)
            {
                collection.Clear();
            }
        }

    }
}