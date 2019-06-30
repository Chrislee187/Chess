using board.engine.Board;

namespace board.engine.Movement
{
    public interface IPathValidator<TEntity> where TEntity : class, IBoardEntity
    {
        Path ValidatePath(IReadOnlyBoardState<TEntity> boardState, Path possiblePath);
    }
}