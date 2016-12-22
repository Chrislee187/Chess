using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpChess.System.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static bool None<TSource>(this IEnumerable<TSource> source) 
            => !source.Any();

        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> selector) 
            => !source.Any(selector);

        public static IEnumerable<string> ToStrings<TSource>(this IEnumerable<TSource> source ) 
            => source.Select(i => i.ToString());
    }


}