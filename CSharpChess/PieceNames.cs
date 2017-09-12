using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CSharpChess
{
    public enum PieceNames { Pawn, Rook, Bishop, Knight, King, Queen, Blank = -9999 }

    public static class PieceNamesExtensions
    {
        private static readonly IDictionary<PieceNames, char> PromotionChars = new Dictionary<PieceNames, char>
        {
            {PieceNames.Rook, 'R'},
            {PieceNames.Bishop, 'B'},
            {PieceNames.Knight, 'N'},
            {PieceNames.Queen, 'Q'}
        };

        public static string ToPromotionCharacter(this PieceNames pieceName)
        {
            return PromotionChars.ContainsKey(pieceName) ? PromotionChars[pieceName].ToString() : "";
        }
    }

}