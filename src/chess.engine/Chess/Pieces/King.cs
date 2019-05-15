using board.engine;
using board.engine.Movement;
using chess.engine.Extensions;
using chess.engine.Game;

namespace chess.engine.Chess.Pieces
{
    public static class King
    {
        public static BoardLocation StartPositionFor(Colours player)
            => player == Colours.White ? "E1".ToBoardLocation() : "E8".ToBoardLocation();
    }
}