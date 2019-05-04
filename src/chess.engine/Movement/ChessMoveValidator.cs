using System;
using System.Collections.Generic;
using System.Linq;
using chess.engine.Board;

namespace chess.engine.Movement
{
    public class ChessPathValidator : IPathValidator
    {
        private readonly IReadOnlyDictionary<MoveType, IEnumerable<BoardMovePredicate>> _validationFactory;

        public ChessPathValidator(IReadOnlyDictionary<MoveType, IEnumerable<BoardMovePredicate>> validationFactory)
        {
            _validationFactory = validationFactory;
        }

        public Path ValidatePath(Path possiblePath, IBoardState boardState)
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

                var moveIsATake = MoveIsATake(move, boardState);

                var moveLeavesKingInCheck = boardState.DoesMoveLeaveMovingPlayersKingInCheck(move);
                // TODO: Does move leave king in check?
                if (!moveLeavesKingInCheck)
                {
                    validPath.Add(move);
                }


                // If move was a take path is blocked so stop here
                if (moveIsATake) break;
            }

            return validPath;
        }

        private static bool MoveIsATake(BoardMove move, IBoardState boardState)
        {
            if (boardState.IsEmpty(move.To)) return false;

            var movePlayerColour = boardState.GetItem(move.From)?.Item.Player;
            var takeEntity = boardState.GetItem(move.To)?.Item;
            var moveIsATake = takeEntity != null && takeEntity.Player != movePlayerColour;
            return moveIsATake;
        }
    }
}