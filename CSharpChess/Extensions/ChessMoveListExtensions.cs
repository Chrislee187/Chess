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
    }

    public static class BoardPieceExtensions
    {
        public static IEnumerable<BoardPiece> EnemyOf(this IEnumerable<BoardPiece> pieces, Chess.Board.Colours player)
        {
            return pieces.Where(p => p.Piece.Is(Chess.ColourOfEnemy(player)));
        }
    }
}