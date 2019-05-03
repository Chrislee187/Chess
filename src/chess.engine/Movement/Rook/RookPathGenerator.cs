using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Pieces;

namespace chess.engine.Movement.Rook
{
    public class RookPathGenerator : StraightLinePathExtender, IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new Paths();

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

        public Paths PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}