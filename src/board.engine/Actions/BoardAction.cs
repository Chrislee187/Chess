using board.engine.Board;
using board.engine.Movement;

namespace board.engine.Actions
{
    public interface IBoardAction
    {
        void Execute(BoardMove move);
    }

    public abstract class BoardAction<TEntity> : IBoardAction where TEntity : class, IBoardEntity
    {
        protected readonly IBoardActionFactory<TEntity> ActionFactory;
        protected readonly IBoardState<TEntity> BoardState;

        protected BoardAction(IBoardActionFactory<TEntity> actionFactory, IBoardState<TEntity> boardState)
        {
            BoardState = boardState;
            ActionFactory = actionFactory;
        }

        public abstract void Execute(BoardMove move);
    }
}