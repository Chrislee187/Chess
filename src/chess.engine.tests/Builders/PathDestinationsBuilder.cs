using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.tests.Builders
{
    public class PathDestinationsBuilder
    {
        private BoardLocation _start;

        private List<(BoardLocation, ChessMoveType)> _destinations = new List<(BoardLocation, ChessMoveType)>();
        public PathDestinationsBuilder(BoardLocation start)
        {
            _start = start;
        }

        public PathDestinationsBuilder To(BoardLocation at, ChessMoveType moveType)
        {
            _destinations.Add((at, moveType));
            return this;
        }
        public PathDestinationsBuilder To(string at, ChessMoveType moveType = ChessMoveType.MoveOnly)
        {
            return To(BoardLocation.At(at), moveType);
        }

        public PathDestinationsBuilder ToPromotion(string at, ChessPieceName promotionPiece)
        {
            return To(BoardLocation.At(at), ChessMoveType.PawnPromotion);
        }

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