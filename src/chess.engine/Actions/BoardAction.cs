using chess.engine.Board;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public interface IBoardAction
    {
        void Execute(BoardMove move);
    }

    public abstract class BoardAction<TEntity> : IBoardAction where TEntity : class, IBoardEntity
    {
        protected readonly IBoardActionFactory<TEntity> Factory;
        protected readonly IBoardState<TEntity> BoardState;

        protected BoardAction(IBoardActionFactory<TEntity> factory, IBoardState<TEntity> boardState)
        {
            BoardState = boardState;
            Factory = factory;
        }

        public abstract void Execute(BoardMove move);
    }
}