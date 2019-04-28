using chess.engine.Game;

namespace chess.engine.tests.Builders
{
    public class PathBuilder
    {
        public PathBuilderDestination From(BoardLocation at) => new PathBuilderDestination(at);

        public PathBuilderDestination From(string at) => From(BoardLocation.At(at));
    }
}