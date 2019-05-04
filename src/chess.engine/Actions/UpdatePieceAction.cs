using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class UpdatePieceAction : BoardAction
    {
        public UpdatePieceAction(IBoardActionFactory factory, IBoardState boardState) : base(factory, boardState)
        {
        }

        public override void Execute(BoardMove move)
        {
            if (BoardState.IsEmpty(move.From)) return;

            var piece = BoardState.GetItem(move.From).Item;
            var forPlayer = piece.Player;

            BoardState.Remove(move.From);

            if (!BoardState.IsEmpty(move.To))
            {
                BoardState.Remove(move.To);
            }
            BoardState.PlaceEntity(move.To, ChessPieceEntityFactory.Create((ChessPieceName)move.UpdateEntityType, forPlayer));
        }
    }
}