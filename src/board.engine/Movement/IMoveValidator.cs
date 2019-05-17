using board.engine.Board;

namespace board.engine.Movement
{
    public interface IMoveValidator<TEntity, TWrapper> where TEntity : class, IBoardEntity
    {
        bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState);
        bool ValidateMove(BoardMove move, TWrapper wrapper);
    }
}