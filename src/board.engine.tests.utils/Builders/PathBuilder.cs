using board.engine.Actions;
using board.engine.Movement;

namespace board.engine.tests.utils.Builders
{
    public class PathBuilder
    {
        public PathDestinationsBuilder From(BoardLocation at) => new PathDestinationsBuilder(at);

        public Path Build()
        {
            return new PathDestinationsBuilder(BoardLocation.At(4, 2))
                .To(BoardLocation.At(4, 4), (int) DefaultActions.MoveOnly)
                .Build();
        }
    }
}