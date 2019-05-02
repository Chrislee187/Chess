using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class EnPassantAction : BoardAction
    {
        public EnPassantAction(IBoardState state, IBoardActionFactory factory) : base(state, factory)
        {
        }

        public override void Execute(ChessMove move)
        {
            var piece = _state.GetEntity(move.From);
            var passedPieceLoc = move.To.MoveBack(piece.Player);

            _state.ClearLocation(passedPieceLoc);
            _factory.Create(DefaultActions.MoveOnly, _state).Execute(move);
        }
    }
}