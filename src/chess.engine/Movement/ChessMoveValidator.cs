using System;
using System.Collections.Generic;
using System.Linq;
using chess.engine.Board;
using chess.engine.Entities;

namespace chess.engine.Movement
{
    public class ChessPathValidator : IPathValidator
    {
        private readonly IReadOnlyDictionary<ChessMoveType, IEnumerable<ChessBoardMovePredicate>> _validationFactory;

        public ChessPathValidator(IReadOnlyDictionary<ChessMoveType, IEnumerable<ChessBoardMovePredicate>> validationFactory)
        {
            _validationFactory = validationFactory;
        }

        public Path ValidatePath(Path possiblePath, IBoardState boardState)
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

        private static bool MoveIsATake(ChessMove move, IBoardState boardState)
        {
            if (boardState.IsEmpty(move.To)) return false;

            var movePlayerColour = boardState.GetItem(move.From)?.Item.Player;
            var takeEntity = boardState.GetItem(move.To)?.Item;
            var moveIsATake = takeEntity != null && takeEntity.Player != movePlayerColour;
            return moveIsATake;
        }
    }
}