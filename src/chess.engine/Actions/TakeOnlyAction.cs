using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class TakeOnlyAction : BoardAction
    {

        public TakeOnlyAction(IBoardState state, BoardActionFactory factory) : base(state, factory)
        {
        }
        public override void Execute(ChessMove move)
        {
            _state.ClearLocation(move.To);

            _factory.Create(ChessMoveType.MoveOnly, _state).Execute(move);
        }
    }
}