using board.engine.Board;

namespace board.engine
{
    public interface IRefreshAllPaths<TEntity> where TEntity : class, IBoardEntity
    {
        void RefreshAllPaths(IBoardState<TEntity> boardState);
    }
}