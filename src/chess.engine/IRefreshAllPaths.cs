using chess.engine.Board;

namespace chess.engine
{
    public interface IRefreshAllPaths<TEntity>
    {
        void RefreshAllPaths(IBoardState<TEntity> boardState, bool removeMovesThatLeaveKingInCheck);
    }
}