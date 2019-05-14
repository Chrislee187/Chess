using System.Collections.Generic;
using System.Linq;
using chess.engine.Game;

namespace chess.engine.Chess.Pieces
{
    public static class PieceNameMapper
    {
        private static readonly IDictionary<char, ChessPieceName> PieceNames = new Dictionary<char, ChessPieceName>
        {
            {'p', ChessPieceName.Pawn },
            {'P', ChessPieceName.Pawn },
            {'r', ChessPieceName.Rook },
            {'R', ChessPieceName.Rook },
            {'n', ChessPieceName.Knight },
            {'N', ChessPieceName.Knight },
            {'b', ChessPieceName.Bishop },
            {'B', ChessPieceName.Bishop },
            {'k', ChessPieceName.King },
            {'K', ChessPieceName.King },
            {'q', ChessPieceName.Queen },
            {'Q', ChessPieceName.Queen },
        };

        public static bool ContainsPiece(char p)
        {
            return PieceNames.ContainsKey(p);
        }
        public static ChessPieceName FromChar(char c)
        {
            return PieceNames[c];
        }
        public static Colours ToOwner(char c)
        {
            return char.IsUpper(c) ? Colours.White : Colours.Black;
        }
        public static char ToChar(ChessPieceName piece, Colours owner)
        {
            var c = PieceNames.FirstOrDefault(n => n.Value == piece).Key;

            return owner == Colours.White ? char.ToUpper(c) : char.ToLower(c);
        }
    }
}