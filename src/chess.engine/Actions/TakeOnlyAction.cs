using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class TakeOnlyAction : BoardAction
    {

        public TakeOnlyAction(IBoardState state) : base(state)
        {
        }
        public override void Execute(ChessMove move)
        {
            TakePieceAction(move.To, _state);

            new MoveOnlyAction(_state).Execute(move);
        }
    }
}