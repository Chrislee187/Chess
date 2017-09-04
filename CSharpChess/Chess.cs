using System.Collections.Generic;

namespace CSharpChess
{
    public static partial class Chess
    {
        public enum Colours { White, Black, None = -9999 }

        public static readonly IEnumerable<Colours> BothColours = new[] {Colours.White, Colours.Black};

        public enum PieceNames { Pawn, Rook, Bishop, Knight, King, Queen, Blank = -9999 }

        public enum GameState
        {
            BlackKingInCheck, WhiteKingInCheck, WaitingForMove, Stalemate,
            Unknown,
            CheckMateBlackWins,
            CheckMateWhiteWins,
            Draw
        }

        // TODO: Unit-Test
        public static Colours ColourOfEnemy(Colours colour) => colour == Colours.Black
            ? Colours.White
            : colour == Colours.White
                ? Colours.Black
                : colour;

        public enum ChessFile { A = 1, B, C, D, E, F, G, H,
            None = 0
        };

        public static IEnumerable<ChessFile> Files => new List<ChessFile> {ChessFile.A, ChessFile.B, ChessFile.C, ChessFile.D, ChessFile.E, ChessFile.F, ChessFile.G, ChessFile.H};
        public static IEnumerable<int> Ranks => new [] {1,2,3,4,5,6,7,8};
    }
}