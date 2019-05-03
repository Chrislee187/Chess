using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement.Bishop;
using chess.engine.Movement.Rook;

namespace chess.engine.Movement.Queen
{
    public class QueenPathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new Paths();
            paths.AddRange(new RookPathGenerator().PathsFrom(location, forPlayer));
            paths.AddRange(new BishopPathGenerator().PathsFrom(location, forPlayer));

            return paths;
        }

        public Paths PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}