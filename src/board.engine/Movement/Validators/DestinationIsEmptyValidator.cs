using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class DestinationIsEmptyValidator<TEntity> : IMoveValidator<TEntity>
        where TEntity : class, IBoardEntity
    {
        public bool ValidateMove(BoardMove move, IReadOnlyBoardState<TEntity> roBoardState) 
            => roBoardState.GetItem(move.To) == null;

    }
}