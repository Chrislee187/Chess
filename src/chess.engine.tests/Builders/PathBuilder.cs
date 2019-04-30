using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.tests.Builders
{
    public class PathBuilder
    {
        public PathDestinationsBuilder From(BoardLocation at) => new PathDestinationsBuilder(at);

        public PathDestinationsBuilder From(string at) => From(BoardLocation.At(at));

        public Path Build()
        {
            return new PathDestinationsBuilder(BoardLocation.At("D2")).To("D4", ChessMoveType.MoveOnly).Build();
        }
    }
}