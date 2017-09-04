using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using static CSharpChess.Chess;

namespace CSharpChess.System.Extensions
{
    public static class BoardPieceExtensions
    {
        public static IEnumerable<BoardPiece> OfColour(this IEnumerable<BoardPiece> pieces, Colours colour) 
            => pieces.Where(p => p.Piece.Is(colour));
    }
}