using board.engine.Board;
using board.engine.Movement;

namespace board.engine
{
    public interface IPathsValidator<TEntity> where TEntity : class, IBoardEntity
    {
        Paths GetValidatedPaths(IBoardState<TEntity> boardState, TEntity entity, BoardLocation boardLocation);
    }
}