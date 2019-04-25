using System.Collections.Generic;
using chess.engine.Pieces;

namespace chess.engine
{
    public interface IPathGenerator
    {
        IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer);
        IEnumerable<Path> PathsFrom(string location, Colours forPlayer);
    }
}