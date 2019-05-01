using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class MoveOrTakeAction : BoardAction
    {


        public MoveOrTakeAction(IBoardState state) : base(state)
        {
        }
        public override void Execute(ChessMove move)
        {
            var dest = _state.GetEntity(move.To);

            if (dest != null)
            {
                TakePieceAction(move.To, _state);
            }

            new MoveOnlyAction(_state).Execute(move);
        }
    }
}