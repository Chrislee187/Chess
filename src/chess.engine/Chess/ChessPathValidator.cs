using System;
using System.Collections.Generic;
using System.Linq;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Movement;

namespace chess.engine.Chess
{
    public class ChessPathValidator : IPathValidator<ChessPieceEntity>
    {
        private readonly IReadOnlyDictionary<MoveType, IEnumerable<BoardMovePredicate<ChessPieceEntity>>> _validationFactory;

        public ChessPathValidator(IReadOnlyDictionary<MoveType, IEnumerable<BoardMovePredicate<ChessPieceEntity>>> validationFactory)
        {
            _validationFactory = validationFactory;
        }

        public Path ValidatePath(IBoardState<ChessPieceEntity> boardState, Path possiblePath)
        {
            var validPath = new Path();
            foreach (var move in possiblePath)
            {
                if (!_validationFactory.TryGetValue(move.MoveType, out var moveTests))
                {
                    throw new ArgumentOutOfRangeException(nameof(move.MoveType), move.MoveType, $"No Move Validator implemented for {move.MoveType}");
                }

                if (!moveTests.All(t => t(move, boardState)))
                {
                    break;
                }

                validPath.Add(move);

                if (PathIsBlocked(move, boardState)) break;
            }

            return validPath;
        }

        private static bool PathIsBlocked(BoardMove move, IBoardState<ChessPieceEntity> boardState)
        {
            if (boardState.IsEmpty(move.To)) return false;

            var movePlayerColour = boardState.GetItem(move.From)?.Item.Player;
            var takeEntity = boardState.GetItem(move.To)?.Item;
            var moveIsATake = takeEntity != null && takeEntity.Player != movePlayerColour;
            return moveIsATake;
        }
    }
}