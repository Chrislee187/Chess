using System.Linq;
using chess.engine.Board;

namespace chess.engine.Movement.Validators
{
    public class DestinationContainsEnemyMoveValidator<TEntity> : IMoveValidator<TEntity> where TEntity : class, IBoardEntity
    {
        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
        {
            var sourcePiece = boardState.GetItems(move.From).SingleOrDefault();
            Guard.NotNull(sourcePiece);
            if (!boardState.IsEmpty(move.To))
            {
                var destinationPiece = boardState.GetItems(move.To).Single();
                return sourcePiece.Item.Owner != destinationPiece.Item.Owner;
            }

            return false;
        }
    }
}