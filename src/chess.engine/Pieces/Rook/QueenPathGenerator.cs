using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.Bishop;

namespace chess.engine.Pieces.Rook
{
    public class QueenPathGenerator : IPathGenerator
    {
        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var straights = new RookPathGenerator();
            var diags = new BishopPathGenerator();

            var paths = new List<Path>();

            paths.AddRange(straights.PathsFrom(location, forPlayer));
            paths.AddRange(diags.PathsFrom(location, forPlayer));

            return paths;
        }

        public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}