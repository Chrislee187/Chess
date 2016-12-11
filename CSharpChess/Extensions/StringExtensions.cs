using System.Collections.Generic;
using CSharpChess.TheBoard;

namespace CSharpChess.Extensions
{
    public static class StringExtensions
    {
        public static string Repeat(this char s, int times)
        {
            return new string(s, times);
        }
    }

    public static class ChessMoveListExtensions
    {
        public static string ToStringList(this IEnumerable<ChessMove> moves) => string.Join(", ", moves);
    }
}