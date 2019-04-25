using chess.engine.Game;
using chess.engine.tests.Pieces;

namespace chess.engine.tests.Builders
{
    public class PathBuilder
    {
        public PathBuilderDestination From(BoardLocation at) => new PathBuilderDestination(at);

        public PathBuilderDestination From(string at) => From(BoardLocation.At(at));
    }
}