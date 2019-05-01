using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class MoveOnlyAction : BoardAction
    {
        public MoveOnlyAction(IBoardState state) : base(state)
        {
        }

        public override void Execute(ChessMove move)
        {
            var piece = _state.GetEntity(move.From);
            _state.ClearLocation(move.From);
            _state.SetEntity(move.To, piece);
        }
    }
}