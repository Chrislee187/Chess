using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.Extensions
{
    public static class ChessMoveListExtensions
    {
        public static string ToStringList(this IEnumerable<ChessMove> moves) 
            => string.Join(", ", moves);

        public static bool ContainsMoveTo(this IEnumerable<ChessMove> moves, BoardLocation loc)
        {
            return moves.Any(m => loc.Equals(m.To));
        }

        public static IEnumerable<ChessMove> Moves(this IEnumerable<ChessMove> moves) =>
            moves.Where(m => m.MoveType.IsMove());

        public static IEnumerable<ChessMove> Covers(this IEnumerable<ChessMove> moves) =>
            moves.Where(m => m.MoveType.IsCover());

        public static IEnumerable<ChessMove> Takes(this IEnumerable<ChessMove> moves) =>
            moves.Where(m => m.MoveType.IsTake());

    }
}