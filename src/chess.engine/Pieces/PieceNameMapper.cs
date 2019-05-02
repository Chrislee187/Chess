using System.Collections.Generic;
using chess.engine.Chess;

namespace chess.engine.Pieces
{
    public static class PieceNameMapper
    {
        private static IDictionary<char, ChessPieceName> _pieceNameMapper = new Dictionary<char, ChessPieceName>
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
        public static ChessPieceName FromChar(char c)
        {
            return _pieceNameMapper[c];
        }
    }
}