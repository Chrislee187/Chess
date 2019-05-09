using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class UpdatePieceValidator<TEntity> : IMoveValidator<TEntity> where TEntity : class, IBoardEntity
    {
        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
        {
            var piece = boardState.GetItem(move.From).Item;

            var destinationIsValid = new DestinationIsEmptyOrContainsEnemyValidator<TEntity>().ValidateMove(move, boardState);
            
            return destinationIsValid;
        }
    }
}