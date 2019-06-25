using System;
using System.Collections.Generic;
using System.Linq;

namespace chess.blazor.Extensions
{
    public static class ExtraLinqExtensions
    {
        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            foreach (T el in list)
            {
                yield return el;
                if (predicate(el))
                    yield break;
            }
        }
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            var enumerable = list as T[] ?? list.ToArray();
            foreach (T el in enumerable)
            {
                action(el);
            }

            return enumerable;
        }
    }
}