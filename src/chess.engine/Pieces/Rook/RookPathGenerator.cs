using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Pieces.Rook
{
    public class RookPathGenerator : StraightLinePathExtender, IPathGenerator
    {
        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new List<Path>();

            foreach (var path in new[]
            {
                ExtendedPathFrom(location, loc => loc.MoveForward(forPlayer)),
                ExtendedPathFrom(location, loc => loc.MoveBack(forPlayer)),
                ExtendedPathFrom(location, loc => loc.MoveLeft(forPlayer)),
                ExtendedPathFrom(location, loc => loc.MoveRight(forPlayer)),
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