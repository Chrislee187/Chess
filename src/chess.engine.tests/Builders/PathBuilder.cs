using chess.engine.Game;

namespace chess.engine.tests.Builders
{
    public class PathBuilder
    {
        public PathDestinationsBuilder From(BoardLocation at) => new PathDestinationsBuilder(at);

        public PathDestinationsBuilder From(string at) => From(BoardLocation.At(at));
    }
}