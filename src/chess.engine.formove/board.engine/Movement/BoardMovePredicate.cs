using board.engine.Board;

namespace board.engine.Movement
{
    public delegate bool BoardMovePredicate<TEntity>(BoardMove move, IReadOnlyBoardState<TEntity> boardState) 
        where TEntity : class, IBoardEntity;
}