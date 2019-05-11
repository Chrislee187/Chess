using System.Collections.Generic;
using System.Linq;
using Chess.Common.System;

namespace Chess.Common.Extensions
{
    public static class BoardPieceExtensions
    {
        public static IEnumerable<BoardPiece> OfColour(this IEnumerable<BoardPiece> pieces, Colours colour) 
            => pieces.Where(p => p.Piece.Is(colour));
    }
}