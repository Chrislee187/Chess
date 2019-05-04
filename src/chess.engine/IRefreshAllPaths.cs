using chess.engine.Board;

namespace chess.engine
{
    public interface IRefreshAllPaths
    {
        void RefreshAllPaths(IBoardState boardState, bool removeMovesThatLeaveKingInCheck);
    }
}