using System.Collections.Generic;
using chess.engine.Game;

namespace chess.engine.Movement
{
    public interface IPathGenerator
    {
        IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer);
        IEnumerable<Path> PathsFrom(string location, Colours forPlayer);
    }
}