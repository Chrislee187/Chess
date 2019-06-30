using board.engine;
using chess.engine.Extensions;
using chess.engine.Game;

namespace chess.engine.Pieces
{
    public static class King
    {
        public static BoardLocation StartPositionFor(Colours player)
            => player == Colours.White ? "E1".ToBoardLocation() : "E8".ToBoardLocation();
    }
}