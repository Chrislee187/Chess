using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess
{
    public static class Chess
    {
        public static IEnumerable<int> Ranks
        {
            get
            {
                for (int rank = 1; rank <= 8; rank++)
                {
                    yield return rank;
                }
            }
        }

        public static IEnumerable<int> Files => Enumerable.Cast<int>(EnumExtensions.All<ChessFile>());

        public enum ChessFile { A=1, B, C, D, E, F, G, H};

        public static class Pieces
        {
            public static readonly ChessPiece Blank = ChessPiece.NullPiece;

            public static class White
            {
                public static readonly ChessPiece Pawn = new ChessPiece(Colours.White, PieceNames.Pawn);
                public static readonly ChessPiece Bishop = new ChessPiece(Colours.White, PieceNames.Bishop);
                public static readonly ChessPiece Knight = new ChessPiece(Colours.White, PieceNames.Knight);
                public static readonly ChessPiece Rook = new ChessPiece(Colours.White, PieceNames.Rook);
                public static readonly ChessPiece King = new ChessPiece(Colours.White, PieceNames.King);
                public static readonly ChessPiece Queen = new ChessPiece(Colours.White, PieceNames.Queen);
            }
            public static class Black
            {
                public static readonly ChessPiece Pawn = new ChessPiece(Colours.Black, PieceNames.Pawn);
                public static readonly ChessPiece Bishop = new ChessPiece(Colours.Black, PieceNames.Bishop);
                public static readonly ChessPiece Knight = new ChessPiece(Colours.Black, PieceNames.Knight);
                public static readonly ChessPiece Rook = new ChessPiece(Colours.Black, PieceNames.Rook);
                public static readonly ChessPiece King = new ChessPiece(Colours.Black, PieceNames.King);
                public static readonly ChessPiece Queen = new ChessPiece(Colours.Black, PieceNames.Queen);
            }
        }

        public enum Colours
        {
            White, Black,
            None
        }

        public enum PieceNames
        {
            Pawn, Rook, Bishop, Knight, King, Queen, Blank = -9999
        }

        public static class Validations
        {
            public static bool InvalidRank(int rank) => !Ranks.Contains(rank);

            public static bool InvalidFile(ChessFile file) => !Files.Contains((int) file);
            public static bool InvalidFile(int file) => !Files.Contains(file);

            public static void ThrowInvalidRank(int rank)
            {
                if (InvalidRank(rank))
                    throw new ArgumentOutOfRangeException(nameof(rank), rank, "Invalid Rank");
            }
            public static void ThrowInvalidFile(int file)
            {
                if (InvalidFile(file))
                    throw new ArgumentOutOfRangeException(nameof(file), file, "Invalid File");
            }

            public static void ThrowInvalidFile(ChessFile file)
            {
                ThrowInvalidFile((int) file);
            }
        }
    }
}