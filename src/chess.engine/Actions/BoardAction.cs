using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public interface IBoardAction
    {
        void Execute(ChessMove move);
    }

    public abstract class BoardAction : IBoardAction
    {
        protected readonly IBoardStateActions _state;
        protected readonly IBoardActionFactory _factory;
        protected BoardAction(IBoardStateActions state, IBoardActionFactory factory)
        {
            _state = state;
            _factory = factory;
        }

        public abstract void Execute(ChessMove move);
    }
}