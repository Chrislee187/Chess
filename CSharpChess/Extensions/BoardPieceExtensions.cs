using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.Extensions
{
    public static class BoardPieceExtensions
    {
        public static IEnumerable<BoardPiece> EnemyOf(this IEnumerable<BoardPiece> pieces, Chess.Board.Colours player)
        {
            return pieces.Where(p => p.Piece.Is(Chess.ColourOfEnemy(player)));
        }
    }
}