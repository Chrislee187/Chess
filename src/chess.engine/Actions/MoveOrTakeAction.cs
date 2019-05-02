using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class MoveOrTakeAction : BoardAction
    {

        public MoveOrTakeAction(IBoardState state, IBoardActionFactory factory) : base(state, factory)
        {
        }
        public override void Execute(ChessMove move)
        {
            var dest = _state.GetEntity(move.To);

            if (dest != null)
            {
                _factory.Create(DefaultActions.TakeOnly, _state).Execute(move);
            }
            else
            {
                _factory.Create(DefaultActions.MoveOnly, _state).Execute(move);
            }

        }
    }
}