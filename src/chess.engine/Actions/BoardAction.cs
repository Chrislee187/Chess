using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public abstract class BoardAction
    {
        protected readonly IBoardState _state;

        protected BoardAction(IBoardState state)
        {
            _state = state;
        }

        public abstract void Execute(ChessMove move);

        protected void TakePieceAction(BoardLocation loc, IBoardState state)
        {
            // TODO: Record lost piece etc.
            state.ClearLocation(loc);
        }
    }
}