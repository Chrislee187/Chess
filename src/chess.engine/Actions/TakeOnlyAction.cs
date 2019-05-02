using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class TakeOnlyAction : BoardAction
    {

        public TakeOnlyAction(IBoardState state, IBoardActionFactory factory) : base(state, factory)
        {
        }
        public override void Execute(ChessMove move)
        {
            _state.ClearLocation(move.To);

            _factory.Create(DefaultActions.MoveOnly, _state).Execute(move);
        }
    }
}