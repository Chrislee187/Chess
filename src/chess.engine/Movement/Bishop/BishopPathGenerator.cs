using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Pieces;

namespace chess.engine.Movement.Bishop
{
    public class BishopPathGenerator : StraightLinePathExtender, IPathGenerator
    {
        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new List<Path>();

            foreach (var path in new[]
            {
                ExtendedPathFrom(location, start => start.MoveForward(forPlayer)?.MoveRight(forPlayer)),
                ExtendedPathFrom(location, start => start.MoveForward(forPlayer)?.MoveLeft(forPlayer)),
                ExtendedPathFrom(location, start => start.MoveBack(forPlayer)?.MoveRight(forPlayer)),
                ExtendedPathFrom(location, start => start.MoveBack(forPlayer)?.MoveLeft(forPlayer))
            })
            {
                if (path != null)
                {
                    paths.Add(path);
                }
            }

            return paths;
        }

        public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);

    }
}