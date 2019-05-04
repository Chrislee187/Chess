using chess.engine.Board;

namespace chess.engine.Movement
{
    public interface IPathValidator<TEntity>
    {
        Path ValidatePath(Path possiblePath, IBoardState<TEntity> boardState);
    }
}