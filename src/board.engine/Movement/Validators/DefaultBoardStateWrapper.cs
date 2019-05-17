using board.engine.Board;

namespace board.engine.Movement.Validators
{
    // NOTE: This class here to try and simplify the long class names
    public abstract class DefaultBoardStateWrapper<TEntity> where TEntity : class, IBoardEntity
    {
        protected IBoardState<TEntity> BoardState;

        protected DefaultBoardStateWrapper(IBoardState<TEntity> boardState)
            => BoardState = boardState;

        public LocatedItem<TEntity> GetFromEntity(BoardMove move) 
            => BoardState.GetFromItem(move);

        public LocatedItem<TEntity> GetToEntity(BoardMove move) 
            => BoardState.GetToItem(move);

        public DestinationIsEmptyOrContainsEnemyValidator<TEntity>.IBoardStateWrapper GetDestinationIsEmptyOrContainsEnemyWrapper() 
            => new DestinationIsEmptyOrContainsEnemyValidator<TEntity>.BoardStateWrapper(BoardState);

        public DestinationIsEmptyValidator<TEntity>.IBoardStateWrapper GetDestinationIsEmptyWrapper() 
            => new DestinationIsEmptyValidator<TEntity>.BoardStateWrapper(BoardState);

        public DestinationContainsEnemyMoveValidator<TEntity>.IBoardStateWrapper GetDestinationContainsEnemyMoveWrapper() 
            => new DestinationContainsEnemyMoveValidator<TEntity>.BoardStateWrapper(BoardState);
        public DestinationNotUnderAttackValidator<TEntity>.IBoardStateWrapper GetDestinationNotUnderAttackWrapper()
            => new DestinationNotUnderAttackValidator<TEntity>.BoardStateWrapper(BoardState);
    }
}