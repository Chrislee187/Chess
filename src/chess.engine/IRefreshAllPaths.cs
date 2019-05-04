using chess.engine.Board;

namespace chess.engine
{
    public interface IRefreshAllPaths
    {
        void RefreshAllPaths<TEntity>(IBoardState<TEntity> boardState, bool removeMovesThatLeaveKingInCheck) where TEntity : IBoardEntity;
    }
}