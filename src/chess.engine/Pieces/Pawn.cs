using System.Collections.Generic;

namespace chess.engine.Pieces
{
    public class Pawn
    {
        public static int StartRankFor(Colours player)
            => player == Colours.White ? 2 : 7;
    }
}