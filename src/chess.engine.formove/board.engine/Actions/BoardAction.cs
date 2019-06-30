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
        protected readonly IBoardActionProvider<TEntity> ActionProvider;
        protected readonly IBoardState<TEntity> BoardState;

        protected BoardAction(IBoardActionProvider<TEntity> actionProvider, IBoardState<TEntity> boardState)
        {
            BoardState = boardState;
            ActionProvider = actionProvider;
        }

        public abstract void Execute(BoardMove move);
    }
}