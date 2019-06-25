using System;
using System.Collections.Generic;
using System.Linq;

namespace chess.engine.Extensions
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
            var forEach = list.ToList();
            forEach.ForEach(action);
            return forEach;
        }
    }
}