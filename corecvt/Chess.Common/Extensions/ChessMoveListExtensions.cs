using System.Collections.Generic;
using System.Linq;

namespace Chess.Common.Extensions
{
    public static class ChessMoveListExtensions
    {
        public static string ToCSV(this IEnumerable<Move> moves) 
            => string.Join(", ", moves);

        public static bool ContainsMoveTo(this IEnumerable<Move> moves, BoardLocation loc) 
            => moves.Any(m => loc.Equals(m.To));

        public static IEnumerable<Move> Moves(this IEnumerable<Move> moves) =>
            moves.Where(m => m.MoveType.IsMove());

        public static IEnumerable<Move> Covers(this IEnumerable<Move> moves) =>
            moves.Where(m => m.MoveType.IsCover());

        public static IEnumerable<Move> Takes(this IEnumerable<Move> moves) =>
            moves.Where(m => m.MoveType.IsTake());

    }
}