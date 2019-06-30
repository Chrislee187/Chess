using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class DestinationContainsEnemyMoveValidator<TEntity>
        : IMoveValidator<TEntity>
        where TEntity : class, IBoardEntity
    {
        public bool ValidateMove(BoardMove move, IReadOnlyBoardState<TEntity> roBoardState)
        {
            var sourcePiece = roBoardState.GetItem(move.From);
            if (sourcePiece == null) return false;

            var destinationPiece = roBoardState.GetItem(move.To);
            if (destinationPiece == null) return false;

            return sourcePiece.Item.Owner != destinationPiece.Item.Owner;
        }
    }
}