using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Net.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> UnionSafe<T>(this IEnumerable<T> collection, IEnumerable<T> union)
        {
            return (collection ?? Enumerable.Empty<T>()).Union(union ?? Enumerable.Empty<T>());
        }

        public static bool IsOneOf<T>(this T item, params T[] collection)
        {
            return collection != null && collection.Contains(item);
        }
        
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>((IEqualityComparer<TKey>) null);
            foreach (var element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        
        public static bool IsNullOrEmpty<T>(this ICollection<T> self) => (self?.Count ?? 0) == 0;
   
        public static void AddRange<T>(this ConcurrentBag<T> bag, IEnumerable<T> collection)
        {
            foreach (var element in collection)
            {
                bag.Add(element);
            }
        }

        public static Dictionary<int, T> ToDictionarySafe<T>(this IEnumerable<T> collection, Func<T, int> keyGetter) 
            => collection == null ? new Dictionary<int, T>() : collection.ToDictionary(keyGetter);
        
        public static IEnumerable<TOut> SelectSafe<TIn, TOut>(this IEnumerable<TIn> collections, Func<TIn, TOut> process, ILogger logger)
        {
            foreach (var item in collections)
            {
                TOut result;
                try
                {
                    result = process(item);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error on processing loop");
                    continue;
                }

                yield return result;
            }
        }
    }
}