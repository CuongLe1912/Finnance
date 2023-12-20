using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace URF.Core.Helper.Extensions
{
    public static class ListExtensions
    {

        public static bool IsNullOrEmpty<T>(this T[] list)
        {
            return list == null || list.Length == 0;
        }

        public static List<T> Clone<T>(this List<T> list) where T : ICloneable
        {
            return list.IsNullOrEmpty() ? null : list.Where(c => c != null).Select(c => (T)c.Clone()).ToList();
        }

        public static List<T> Shuffle<T>(this List<T> items)
        {
            if (items == null || items.Count <= 1) return items;

            var random = new Random();
            return items.Select(s => new KeyValuePair<int, T>(random.Next(), s))
                .OrderBy(item => item.Key)
                .Select(c => c.Value)
                .ToList();
        }

        public static bool IsNullOrEmpty(this Hashtable hash)
        {
            return hash == null || hash.Count == 0;
        }

        public static bool IsNullOrEmpty(this string[] array)
        {
            return array == null || array.Length == 0;
        }

        public static bool IsNullOrEmpty(this DateTime[] array)
        {
            return array == null || array.Length == 0;
        }

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this HashSet<T> hash)
        {
            return hash == null || hash.Count == 0;
        }

        public static bool IsNullOrEmpty<T, V>(this Dictionary<T, V> keyValues)
        {
            return keyValues == null || keyValues.Count == 0;
        }

        public static List<IDictionary<string, object>> ToListDictionary(this IEnumerable keyValues)
        {
            var dictionaries = new List<IDictionary<string, object>>();
            foreach (var item in keyValues)
            {
                var dic = item.ToDictionary();
                if (dic != null)
                    dictionaries.Add(dic);
            }
            return dictionaries;
        }
    }
}