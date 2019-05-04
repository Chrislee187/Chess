using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.tests.Builders
{
    public class PathDestinationsBuilder
    {
        private readonly BoardLocation _start;

        private readonly List<(BoardLocation, MoveType)> _destinations = new List<(BoardLocation, MoveType)>();
        public PathDestinationsBuilder(BoardLocation start)
        {
            _start = start;
        }

        public PathDestinationsBuilder To(BoardLocation at, MoveType moveType)
        {
            _destinations.Add((at, moveType));
            return this;
        }
        public PathDestinationsBuilder To(string at, MoveType moveType = MoveType.MoveOnly)
        {
            return To(BoardLocation.At(at), moveType);
        }

        public PathDestinationsBuilder ToUpdatePiece(string at, ChessPieceName promotionPiece)
        {
            return To(BoardLocation.At(at), MoveType.UpdatePiece);
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