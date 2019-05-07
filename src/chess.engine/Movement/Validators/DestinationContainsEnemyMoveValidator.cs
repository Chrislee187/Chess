using System;
using System.Linq;
using chess.engine.Board;

namespace chess.engine.Movement.Validators
{
    public class DestinationContainsEnemyMoveValidator<TEntity> : IMoveValidator<TEntity> where TEntity : class, IBoardEntity
    {
        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
        {
            if (boardState.IsEmpty(move.From)) throw new SystemException($"Piece missing at {move.From}");
            var sourcePiece = boardState.GetItems(move.From).Single();

            if (!boardState.IsEmpty(move.To))
            {
                var destinationPiece = boardState.GetItems(move.To).Single();
                return sourcePiece.Item.Owner != destinationPiece.Item.Owner;
            }

            return false;
        }
    }
}