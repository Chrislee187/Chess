using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Game;

namespace chess.engine.Movement.ChessPieces.Knight
{
    public class KnightPathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, int forPlayer)
        {
            var paths = new Paths();

            var playerIdx = (Colours) forPlayer;
            foreach (var dest in new[]
            {
                location.KnightVerticalMove(playerIdx, true, true),
                location.KnightVerticalMove(playerIdx, true, false),
                location.KnightVerticalMove(playerIdx, false, true),
                location.KnightVerticalMove(playerIdx, false, false),

                location.KnightHorizontalMove(playerIdx, true, true),
                location.KnightHorizontalMove(playerIdx, true, false),
                location.KnightHorizontalMove(playerIdx, false, true),
                location.KnightHorizontalMove(playerIdx, false, false),
            })
            {
                if (dest != null)
                {
                    paths.Add(new Path { BoardMove.Create(location, dest, (int)DefaultActions.MoveOrTake) });
                }
            }

            return paths;
        }
    }
}