using System.Collections.Generic;
using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Chess;
using chess.engine.Extensions;

namespace chess.engine.tests.Builders
{
    public class ChessPathDestinationsBuilder
    {
        private readonly BoardLocation _start;

        private readonly List<(BoardLocation, int)> _destinations = new List<(BoardLocation, int)>();
        public ChessPathDestinationsBuilder(BoardLocation start)
        {
            _start = start;
        }

        public ChessPathDestinationsBuilder To(BoardLocation at, int chessMoveTypes)
        {
            _destinations.Add((at, chessMoveTypes));
            return this;
        }
        public ChessPathDestinationsBuilder To(string at, int chessMoveTypes = (int) DefaultActions.MoveOnly)
        {
            return To((BoardLocation) at.ToBoardLocation(), chessMoveTypes);
        }

        public ChessPathDestinationsBuilder ToUpdatePiece(string at, ChessPieceName promotionPiece)
        {
            return To((BoardLocation) at.ToBoardLocation(), (int) DefaultActions.UpdatePiece);
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