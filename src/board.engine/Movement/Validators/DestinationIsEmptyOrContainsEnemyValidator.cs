using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class DestinationIsEmptyOrContainsEnemyValidator<TEntity> 
        : IMoveValidator<TEntity> 
        where TEntity : class, IBoardEntity
    {

        public bool ValidateMove(BoardMove move, IReadOnlyBoardState<TEntity> roBoardState)
        {
            return new DestinationIsEmptyValidator<TEntity>().ValidateMove(move, roBoardState)
                   || new DestinationContainsEnemyMoveValidator<TEntity>().ValidateMove(move, roBoardState);
        }
    }
}