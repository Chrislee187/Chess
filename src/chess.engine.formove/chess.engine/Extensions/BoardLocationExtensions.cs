using System.Collections.Generic;
using board.engine;

namespace chess.engine.Extensions
{
    public static class BoardLocationExtensions
    {
        public static string ToChessCoord(this BoardLocation loc)
        {
            return $"{(char)('A' + loc.X - 1)}{loc.Y}".ToLower();
        }
    }
}