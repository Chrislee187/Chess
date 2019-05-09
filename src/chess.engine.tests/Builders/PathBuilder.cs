using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Extensions;

namespace chess.engine.tests.Builders
{
    public class PathBuilder
    {
        public PathDestinationsBuilder From(BoardLocation at) => new PathDestinationsBuilder(at);

        public PathDestinationsBuilder From(string at) => From(at.ToBoardLocation());

        public Path Build()
        {
            return new PathDestinationsBuilder("D2".ToBoardLocation()).To("D4".ToBoardLocation(), (int) DefaultActions.MoveOnly).Build();
        }
    }
}