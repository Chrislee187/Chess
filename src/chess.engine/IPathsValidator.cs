using chess.engine.Board;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine
{
    public interface IPathsValidator<TEntity> where TEntity : class, IBoardEntity
    {
        Paths GeneratePossiblePaths(IBoardState<TEntity> boardState, TEntity entity, BoardLocation boardLocation);
    }
}