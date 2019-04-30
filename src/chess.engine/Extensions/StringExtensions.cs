using System;
using System.Collections.Generic;

namespace chess.engine.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitInParts(this string str, int partLength)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < str.Length; i += partLength)
                yield return str.Substring(i, Math.Min(partLength, str.Length - i));
        }
    }
}