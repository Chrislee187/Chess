using chess.engine.Board;

namespace chess.engine
{
    public interface IRefreshAllPaths<TEntity> where TEntity : class, IBoardEntity
    {
        void RefreshAllPaths(IBoardState<TEntity> boardState);
    }
}