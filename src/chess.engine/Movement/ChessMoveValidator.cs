using System;
using System.Linq;
using chess.engine.Board;

namespace chess.engine.Movement
{
    public class ChessMoveValidator : IMoveValidator
    {
        private static readonly MoveValidationFactory ValidationFactory = new MoveValidationFactory();
        public Path ValidPath(Path possiblePath, BoardState boardState)
        {
            var validPath = new Path();
            foreach (var move in possiblePath)
            {
                if (!ValidationFactory.TryGetValue(move.ChessMoveType, out var moveTests))
                {
                    throw new ArgumentOutOfRangeException(nameof(move.ChessMoveType), move.ChessMoveType, $"No Move Validator implemented for {move.ChessMoveType}");
                }

                if (!moveTests.All(t => t(move, boardState)))
                {
                    break;
                }

                validPath.Add(move);
            }

            return validPath;
        }
    }
}