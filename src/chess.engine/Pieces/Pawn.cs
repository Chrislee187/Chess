using chess.engine.Game;

namespace chess.engine.Pieces
{
    public class Pawn
    {
        public static int StartRankFor(Colours player)
            => player == Colours.White ? 2 : 7;

        public static int EnPassantRankFor(Colours player)
            => player == Colours.White ? 5 : 4;

    }
}