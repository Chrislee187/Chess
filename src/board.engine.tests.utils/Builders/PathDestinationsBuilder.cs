using System.Collections.Generic;
using board.engine.Actions;
using board.engine.Movement;

namespace board.engine.tests.utils.Builders
{
    public class PathDestinationsBuilder
    {
        private readonly BoardLocation _start;

        private readonly List<(BoardLocation, int)> _destinations = new List<(BoardLocation, int)>();
        public PathDestinationsBuilder(BoardLocation start)
        {
            _start = start;
        }

        public PathDestinationsBuilder To(BoardLocation at, int chessMoveTypes)
        {
            _destinations.Add((at, chessMoveTypes));
            return this;
        }

        public PathDestinationsBuilder ToUpdatePiece(BoardLocation at, object  promotionPiece)
        {
            return To((BoardLocation) at, (int) DefaultActions.UpdatePiece);
        }

        public Path Build()
        {
            var path = new Path();

            foreach (var destination in _destinations)
            {
                path.Add(BoardMove.Create(_start, destination.Item1, destination.Item2));
            }

            return path;
        }
    }
}