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
            CheckMateWhiteWins
        }
        // TODO: Unit-Test
        public static Colours ColourOfEnemy(Colours colour) => colour == Colours.Black
            ? Colours.White
            : colour == Colours.White
                ? Colours.Black
                : colour;
    }
}