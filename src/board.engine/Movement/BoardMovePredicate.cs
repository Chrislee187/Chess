using board.engine.Board;

namespace board.engine.Movement
{
    public delegate bool BoardMovePredicate<TEntity>(BoardMove move, IBoardState<TEntity> boardState) 
        where TEntity : class, IBoardEntity;
}