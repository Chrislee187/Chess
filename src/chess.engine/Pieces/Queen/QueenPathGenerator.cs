using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.Bishop;
using chess.engine.Pieces.Rook;

namespace chess.engine.Pieces.Queen
{
    public class QueenPathGenerator : IPathGenerator
    {
        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new List<Path>();
            paths.AddRange(new RookPathGenerator().PathsFrom(location, forPlayer));
            paths.AddRange(new BishopPathGenerator().PathsFrom(location, forPlayer));

            return paths;
        }

        public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}