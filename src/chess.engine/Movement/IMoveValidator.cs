using chess.engine.Board;

namespace chess.engine.Movement
{
    public interface IMoveValidator<TEntity>
    {
        bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState);
    }
}