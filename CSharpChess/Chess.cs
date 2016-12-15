using System.Collections.Generic;

namespace CSharpChess
{
    // TODO: Needs Love, partial statics?
    public static partial class Chess
    {
        public enum Colours { White, Black, None = -9999 }

        public static IEnumerable<Colours> BothColours = new[] {Chess.Colours.White, Chess.Colours.Black};

        public enum PieceNames { Pawn, Rook, Bishop, Knight, King, Queen, Blank = -9999 }


        public enum GameState
        {
            BlackKingInCheck, WhiteKingInCheck, WaitingForMove, CheckMate, Stalemate,
            Unknown,
            CheckMateBlackWins,
            CheckMateWhiteWins
        }

        public static Colours ColourOfEnemy(Colours colour) => colour == Colours.Black
            ? Colours.White
            : colour == Colours.White
                ? Colours.Black
                : colour;
    }
}