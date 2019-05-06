using System;
using chess.engine.Board;

namespace chess.engine
{
    public interface IRefreshAllPaths<TEntity> where TEntity : class, ICloneable
    {
        void RefreshAllPaths(IBoardState<TEntity> boardState);
    }
}