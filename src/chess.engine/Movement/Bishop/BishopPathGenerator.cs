using board.engine;
using board.engine.Movement;
using chess.engine.Game;

namespace chess.engine.Movement.Bishop
{
    public class BishopPathGenerator : StraightLinePathExtender, IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, int forPlayer)
        {
            var paths = new Paths();

            var playerIdx = (Colours)forPlayer;
            foreach (var path in new[]
            {
                ExtendedPathFrom(location, start => start.MoveForward(playerIdx)?.MoveRight(playerIdx)),
                ExtendedPathFrom(location, start => start.MoveForward(playerIdx)?.MoveLeft(playerIdx)),
                ExtendedPathFrom(location, start => start.MoveBack(playerIdx)?.MoveRight(playerIdx)),
                ExtendedPathFrom(location, start => start.MoveBack(playerIdx)?.MoveLeft(playerIdx))
            })
            {
                if (path != null)
                {
                    paths.Add(path);
                }
            }

            return paths;
        }
    }
}