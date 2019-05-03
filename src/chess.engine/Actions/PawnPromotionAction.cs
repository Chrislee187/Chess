using chess.engine.Entities;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class PawnPromotionAction : BoardAction
    {
        public PawnPromotionAction(IBoardStateActions state, IBoardActionFactory factory) : base(state, factory)
        {
        }

        public override void Execute(ChessMove move)
        {
            var forPlayer = _state.GetEntity(move.From).Player;
            _state.ClearLocation(move.From);

            if (_state.GetEntity(move.To) != null)
            {
                _state.ClearLocation(move.To);
            }
            _state.SetEntity(move.To, ChessPieceEntityFactory.Create(move.PromotionPiece, forPlayer));
        }
    }
}