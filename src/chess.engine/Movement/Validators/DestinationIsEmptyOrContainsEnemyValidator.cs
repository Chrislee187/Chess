using chess.engine.Board;

namespace chess.engine.Movement.Validators
{
    public class DestinationIsEmptyOrContainsEnemyValidator<TEntity> : IMoveValidator<TEntity> where TEntity : IBoardEntity
    {
        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
            => new DestinationIsEmptyValidator<TEntity>().ValidateMove(move, boardState) 
               || new DestinationContainsEnemyMoveValidator<TEntity>().ValidateMove(move, boardState);

    }
}