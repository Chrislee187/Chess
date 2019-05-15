using System;
using System.Collections.Generic;
using board.engine;
using chess.engine.Algebraic;
using chess.engine.Game;

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
            if (!Enum.TryParse(s[0].ToString().ToUpper(), out ChessFile x)) throw new ArgumentException($"Invalid BoardLocation {s}");
            if (!int.TryParse(s[1].ToString(), out var y)) throw new ArgumentException($"Invalid BoardLocation {s}");

            return BoardLocation.At((int)x, y);
        }

        public static StandardAlgebraicNotation ToSan(this string s)
        {
            return StandardAlgebraicNotation.Parse(s);
        }

    }
}