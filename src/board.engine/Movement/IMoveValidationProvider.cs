using System.Collections.Generic;
using board.engine.Board;

namespace board.engine.Movement
{
    public interface IMoveValidationProvider<TEntity> 
        where TEntity : class, IBoardEntity
    {
        IEnumerable<BoardMovePredicate<TEntity>> Create(int chessMoveTypes, IBoardState<TEntity> boardState);
        bool TryGetValue(int moveType, out IEnumerable<BoardMovePredicate<TEntity>> validators);
    }
}