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
        /// <summary>
        /// Merges the item into the target list if it doesn't exist already
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="item"></param>
        public static void Merge<T>(this IList<T> target, T item)
        {
            if (item == null)
                return;

            if (target == null)
                return;

            if (target.Contains(item))
                return;

            target.Add(item);
        }

        /// <summary>
        /// Merges the contents of the source list into the target list, without any duplicates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static void Merge<T>(IList<T> target, IList<T> source)
        {
            if (source == null)
                return;

            if (target == null)
                return;

            int maxItems = source.Count;
            for (int i = 0; i < maxItems; i++)
            {
                T item = source[i];

                if (target.Contains(item))
                    continue;

                target.Add(item);
            }
        }

        ///// <summary>
        ///// Returns the number of items in the given list
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="items"></param>
        //
        //internal static int Count<T>(this IEnumerable<T> items)
        //{
        //    if (items == null)
        //    {
        //        throw new ArgumentNullException("items");
        //    }

        //    var is2 = items as ICollection<T>;
        //    if (is2 != null)
        //    {
        //        return is2.Count;
        //    }

        //    var is3 = items as ICollection;
        //    if (is3 != null)
        //    {
        //        return is3.Count;
        //    }

        //    int num = 0;
        //    using (var enumerator = items.GetEnumerator())
        //    {
        //        while (enumerator.MoveNext())
        //        {
        //            num++;
        //        }
        //    }
        //    return num;
        //}

        ///// <summary>
        ///// Returns TRUE if the item is in the list
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="items"></param>
        ///// <param name="item"></param>
        //
        //internal static bool Contains<T>(this IEnumerable<T> items, T item)
        //{
        //    if (items == null)
        //    {
        //        throw new ArgumentNullException("items");
        //    }

        //    var is2 = items as ICollection<T>;
        //    if (is2 != null)
        //    {
        //        return is2.Contains(item);
        //    }

        //    var is3 = items as IList;
        //    if (is3 != null)
        //    {
        //        return is3.Contains(item);
        //    }

        //    using (var enumerator = items.GetEnumerator())
        //    {
        //        while (enumerator.MoveNext())
        //        {
        //            if (object.Equals(enumerator.Current, item))
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        public static bool DoesNotContain<T>(this IEnumerable<T> items, T item)
        {
            return System.Linq.Enumerable.Contains(items, item) == false;
        }

        /// <summary>
        /// Returns TRUE if the item exists in the dictionary. This can perform MUCH faster than ContainsKey() or ContainsValue()
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="items"></param>
        /// <param name="key"></param>
        
        public static bool Contains<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key)
        {
            TValue dummy;
            return items.TryGetValue(key, out dummy);
        }

        /// <summary>
        /// Returns TRUE if the item does NOT exist in the dictionary. This performs MUCH faster than ContainsKey() or ContainsValue()
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="items"></param>
        /// <param name="key"></param>
        
        public static bool DoesNotContain<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key)
        {
            return Contains(items, key) == false;
        }

        /// <summary>
        /// Like TryGetValue, but doesn't use 'out' parameters
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="items"></param>
        /// <param name="key"></param>
        /// <returns>Returns the matching item, or NULL if not found</returns>
        public static TValue TryGetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key)
        {
            TValue val;
            return items.TryGetValue(key, out val) ? val : default(TValue);
        }

        /// <summary>
        /// Like TryGetValue, but doesn't use 'out' parameters
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="items"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue">The value to return if the item was not found</param>
        /// <returns>Returns the matching item, or defaultValue if not found</returns>
        public static TValue TryGetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> items, TKey key, TValue defaultValue)
        {
            TValue val;
            return items.TryGetValue(key, out val) ? val : defaultValue;
        }

        /// <summary>
        /// Null safe version of IList.Clear()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
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
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void ClearSafely<T>(this ICollection<T> collection)
        {
            if (collection != null)
            {
                collection.Clear();
            }
        }
        //
        //        /// <summary>
        //        /// Null safe version of ReadOnlyDictionary.ClearItems()
        //        /// </summary>
        //        /// <typeparam name="T"></typeparam>
        //        /// <param name="list"></param>
        //        public static void ClearSafely<TKey, TValue>(this ReadOnlyDictionary<TKey, TValue> dictionary)
        //        {
        //            if (dictionary != null)
        //            {
        //                dictionary.ClearItems();
        //            }
        //        }

    }
}