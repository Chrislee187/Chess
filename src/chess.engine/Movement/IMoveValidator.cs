using chess.engine.Board;

namespace chess.engine.Movement
{
    public interface IMoveValidator<TEntity> where TEntity : class, IBoardEntity
    {
        bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState);
    }
}