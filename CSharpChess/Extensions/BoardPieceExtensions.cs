using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.Extensions
{
    public static class BoardPieceExtensions
    {
        public static IEnumerable<BoardPiece> OfColour(this IEnumerable<BoardPiece> pieces, Chess.Board.Colours colour) 
            => pieces.Where(p => p.Piece.Is(colour));
    }
}