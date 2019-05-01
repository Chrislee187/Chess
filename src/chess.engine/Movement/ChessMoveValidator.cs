using System;
using System.Collections.Generic;
using System.Linq;
using chess.engine.Board;

namespace chess.engine.Movement
{
    public class ChessPathValidator : IPathValidator
    {
        private readonly IReadOnlyDictionary<ChessMoveType, IEnumerable<ChessBoardMovePredicate>> _validationFactory;

        public ChessPathValidator(IReadOnlyDictionary<ChessMoveType, IEnumerable<ChessBoardMovePredicate>> validationFactory)
        {
            _validationFactory = validationFactory;
        }

        public Path ValidatePath(Path possiblePath, BoardState boardState)
        {
            var validPath = new Path();
            foreach (var move in possiblePath)
            {
                if (!_validationFactory.TryGetValue(move.ChessMoveType, out var moveTests))
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