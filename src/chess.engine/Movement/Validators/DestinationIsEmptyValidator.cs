using chess.engine.Board;

namespace chess.engine.Movement.Validators
{
    public class DestinationIsEmptyValidator<TEntity> : IMoveValidator<TEntity>
    {
        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
            => boardState.IsEmpty(move.To);

    }
}