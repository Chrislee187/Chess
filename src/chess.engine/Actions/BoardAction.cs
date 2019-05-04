using chess.engine.Board;
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
        protected readonly IBoardActionFactory Factory;
        protected readonly IBoardState BoardState;

        protected BoardAction(IBoardActionFactory factory, IBoardState boardState)
        {
            BoardState = boardState;
            Factory = factory;
        }

        public abstract void Execute(ChessMove move);
    }
}