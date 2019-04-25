using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.tests.Builders
{
    public class PathBuilderDestination
    {
        private BoardLocation _start;

        private List<(BoardLocation, ChessMoveType)> _destinations = new List<(BoardLocation, ChessMoveType)>();
        public PathBuilderDestination(BoardLocation start)
        {
            _start = start;
        }

        public PathBuilderDestination To(BoardLocation at, ChessMoveType moveType)
        {
            _destinations.Add((at, moveType));
            return this;
        }
        public PathBuilderDestination To(string at, ChessMoveType moveType) => To(BoardLocation.At(at), moveType);

        public Path Build()
        {
            var path = new Path();

            foreach (var destination in _destinations)
            {
                path.Add(ChessMove.Create(_start, destination.Item1, destination.Item2));
            }

            return path;
        }
    }
}