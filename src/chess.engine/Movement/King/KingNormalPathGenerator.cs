using board.engine;
using board.engine.Movement;
using chess.engine.Game;

namespace chess.engine.Movement.King
{
    public class KingNormalPathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, int forPlayer)
        {
            var paths = new Paths();
            var playerIdx = (Colours) forPlayer;
            foreach (var dest in new[]
            {
                location.MoveForward(playerIdx),
                location.MoveForward(playerIdx)?.MoveRight(playerIdx),
                location.MoveRight(playerIdx),
                location.MoveBack(playerIdx)?.MoveRight(playerIdx),
                location.MoveBack(playerIdx),
                location.MoveBack(playerIdx)?.MoveLeft(playerIdx),
                location.MoveLeft(playerIdx),
                location.MoveLeft(playerIdx)?.MoveForward(playerIdx)
            })
            {
                if (dest != null)
                {
                    paths.Add(new Path { BoardMove.Create(location, dest, (int)ChessMoveTypes.KingMove) });
                }
            }

            return paths;
        }
    }
}