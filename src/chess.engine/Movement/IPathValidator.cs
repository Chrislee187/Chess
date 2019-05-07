using System;
using chess.engine.Board;

namespace chess.engine.Movement
{
    public interface IPathValidator<TEntity> where TEntity : class, IBoardEntity
    {
        Path ValidatePath(IBoardState<TEntity> boardState, Path possiblePath);
    }
}