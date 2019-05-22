using System;
using System.Collections.Generic;
using System.IO;
using board.engine;
using chess.engine.Game;
using chess.engine.SAN;

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

        public static BoardLocation ToBoardLocation(this string s)
        {
            if (s.Length != 2) throw new ArgumentException($"Invalid BoardLocation {s}");

            // TODO: This is nice helper function but does make it dependent on ChessFile a Chess specific!!!
            if (!Enum.TryParse(s[0].ToString().ToUpper(), out ChessFile x))
                throw new ArgumentException($"Invalid BoardLocation {s}");
            if (!int.TryParse(s[1].ToString(), out var y)) throw new ArgumentException($"Invalid BoardLocation {s}");

            return BoardLocation.At((int) x, y);
        }

        public static StandardAlgebraicNotation ToSan(this string s)
        {
            return StandardAlgebraicNotation.Parse(s);
        }

        public static Stream ToStream(this string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        public static int ToInt(this string text)
        {
            int temp;
            int.TryParse(text, out temp);
            return temp;
        }
    }

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
    }
}

